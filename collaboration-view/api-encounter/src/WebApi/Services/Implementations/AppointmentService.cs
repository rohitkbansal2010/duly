// -----------------------------------------------------------------------
// <copyright file="AppointmentService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Configurations;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Extensions;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IAppointmentService"/>
    /// </summary>
    internal class AppointmentService : IAppointmentService
    {
        private static readonly HumanName UnknownPatientHumanName = new() { FamilyName = "Unknown" };
        private static readonly HumanName BtgPatientHumanName = new() { FamilyName = "Restricted" };

        private static readonly IReadOnlyDictionary<string, Models.Patient> EmptyPatientDictionary = new Dictionary<string, Models.Patient>(0);
        private static readonly IReadOnlyDictionary<string, Models.PractitionerGeneralInfo> EmptyPractitionerDictionary = new Dictionary<string, Models.PractitionerGeneralInfo>(0);

        private readonly HashSet<string> _newPatientVisitTypes;
        private readonly string[] _includedAppointmentVisitTypes;
        private readonly Models.AppointmentStatusParam[] _excludedAppointmentStatuses;
        private readonly Models.AppointmentStatusParam[] _includedPatientAppointmentStatuses;
        private readonly HashSet<Models.AppointmentStatus> _recentPatientAppointmentStatuses;
        private readonly HashSet<Models.AppointmentStatus> _upcomingPatientAppointmentStatuses;

        private readonly IAppointmentRepository _repository;
        private readonly IPatientRepository _patientRepository;
        private readonly IPractitionerRepository _practitionerRepository;
        private readonly ILogger<AppointmentService> _logger;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository repository,
            IMapper mapper,
            IPatientRepository patientRepository,
            IPractitionerRepository practitionerRepository,
            ILogger<AppointmentService> logger,
            IOptionsMonitor<AppointmentOptions> optionsMonitor)
        {
            _repository = repository;
            _patientRepository = patientRepository;
            _practitionerRepository = practitionerRepository;
            _logger = logger;
            _mapper = mapper;
            _newPatientVisitTypes = optionsMonitor.CurrentValue.ConvertedNewPatientVisitTypes;
            _includedAppointmentVisitTypes = optionsMonitor.CurrentValue.ConvertedIncludedAppointmentVisitTypes;
            _excludedAppointmentStatuses = optionsMonitor.CurrentValue.ConvertedExcludedAppointmentStatuses;
            _includedPatientAppointmentStatuses = optionsMonitor.CurrentValue.ConvertedIncludedPatientAppointmentStatuses;
            _recentPatientAppointmentStatuses = optionsMonitor.CurrentValue.ConvertedRecentPatientAppointmentStatuses;
            _upcomingPatientAppointmentStatuses = optionsMonitor.CurrentValue.ConvertedUpcomingPatientAppointmentStatuses;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsBySiteIdAndDateRangeAsync(string siteId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var ngdpAppointments = (await _repository.GetAppointmentsBySiteIdAndDateRangeAsync(
                    siteId,
                    startDate,
                    endDate,
                    _includedAppointmentVisitTypes,
                    _excludedAppointmentStatuses))
                .ToArray();

            if (!ngdpAppointments.Any())
            {
                return Enumerable.Empty<Appointment>();
            }

            var epicPatientsTask = GetEpicPatientsAsync(ngdpAppointments);
            var epicPractitionersTask = GetEpicPractitionersByIdentitiesAsync(ngdpAppointments);

            await Task.WhenAll(epicPatientsTask, epicPractitionersTask);

            var epicPatients = epicPatientsTask.Result;
            var epicPractitioners = epicPractitionersTask.Result;

            var contractAppointments = GetContractAppointments(ngdpAppointments, epicPatients, epicPractitioners);

            return contractAppointments;
        }

        public async Task<PatientAppointments> GetAppointmentsForSamePatientByAppointmentIdAsync(string appointmentId)
        {
            var ngdpAppointments = (await _repository.GetAppointmentsForSamePatientByAppointmentIdAsync(
                    appointmentId,
                    DateTimeOffset.UtcNow.AddYears(-3),
                    DateTimeOffset.UtcNow.AddYears(3),
                    _includedPatientAppointmentStatuses))
                .OrderBy(appointment => appointment.TimeSlot.StartTime)
                .ToArray();

            if (ngdpAppointments.Length <= 1)
            {
                return PatientAppointments.BuildEmptyPatientAppointments();
            }

            return await GetAppointmentsForSamePatientByAppointmentIdAsync(appointmentId, ngdpAppointments);
        }

        public async Task<PractitionerGeneralInfo> GetPractitionerByAppointmentIdAsync(string appointmentId)
        {
            var ngdpAppointment = await _repository.GetAppointmentByIdAsync(appointmentId);

            var epicPractitioners = await GetEpicPractitionersAsync(ngdpAppointment);

            var practitionerInfo = GetPractitionerGeneralInfo(epicPractitioners, ngdpAppointment);
            return practitionerInfo;
        }

        private Appointment[] GetContractAppointments(
            Models.Appointment[] ngdpAppointments,
            IReadOnlyDictionary<string, Models.Patient> epicPatients,
            IReadOnlyDictionary<string, Models.PractitionerGeneralInfo> epicPractitioners)
        {
            var resultAppointments = ngdpAppointments
                .OrderBy(a => a.TimeSlot.StartTime)
                .ThenBy(a => a.TimeSlot.EndTime)
                .Select(appointment => new Appointment
                {
                    Id = appointment.Id,
                    Status = AppointmentStatus.Unknown,
                    Type = appointment.IsTelehealthVisit
                        ? AppointmentType.Telehealth
                        : AppointmentType.OnSite,
                    Title = appointment.Visit.TypeDisplayName,
                    Patient = new PatientExtendedInfo
                    {
                        PatientGeneralInfo = GetPatientGeneralInfo(epicPatients, appointment),
                        IsNewPatient = _newPatientVisitTypes.Contains(appointment.Visit.TypeId)
                    },
                    Practitioner = GetPractitionerGeneralInfo(epicPractitioners, appointment),
                    TimeSlot = _mapper.Map<TimeSlot>(appointment.TimeSlot),
                    IsProtectedByBtg = appointment.IsProtectedByBtg
                })
                .ToArray();

            return resultAppointments;
        }

        private async Task<PatientAppointments> GetAppointmentsForSamePatientByAppointmentIdAsync(
            string appointmentId,
            Models.Appointment[] ngdpAppointments)
        {
            var index = Array.FindIndex(ngdpAppointments, appointment => appointment.Id == appointmentId);

            var recent = ngdpAppointments
                .Take(index)
                .Where(appointment => _recentPatientAppointmentStatuses.Contains(appointment.Status)
                                      && appointment.Id != appointmentId)
                .Reverse()
                .ToArray();

            var upcoming = ngdpAppointments
                .Skip(index)
                .Where(appointment => _upcomingPatientAppointmentStatuses.Contains(appointment.Status)
                                      && appointment.Id != appointmentId)
                .ToArray();

            var epicPractitioners = await GetEpicPractitionersByIdentitiesAsync(recent.Union(upcoming));

            return new PatientAppointments
            {
                RecentAppointments = GetAppointmentsGroups(recent, epicPractitioners),
                UpcomingAppointments = GetAppointmentsGroups(upcoming, epicPractitioners)
            };
        }

        private PatientAppointmentsGroup[] GetAppointmentsGroups(
            IEnumerable<Models.Appointment> ngdpAppointments,
            IReadOnlyDictionary<string, Models.PractitionerGeneralInfo> epicPractitioners)
        {
            return ngdpAppointments
                .GroupBy(
                    x => x.Visit.TypeDisplayName,
                    x => x,
                    (title, groupedNgdpAppointments) =>
                        BuildPatientAppointmentsGroup(epicPractitioners, title, groupedNgdpAppointments))
                .ToArray();
        }

        private PatientAppointmentsGroup BuildPatientAppointmentsGroup(
            IReadOnlyDictionary<string, Models.PractitionerGeneralInfo> epicPractitioners,
            string groupTitle,
            IEnumerable<Models.Appointment> groupedNgdpAppointments)
        {
            var appointments = groupedNgdpAppointments.ToArray();
            var firstAppointment = appointments.First();
            var lastAppointment = appointments.Last();

            return new PatientAppointmentsGroup
            {
                Title = groupTitle,
                Appointments = BuildPatientAppointments(appointments, epicPractitioners),
                NearestAppointmentDate = firstAppointment.TimeSlot.StartTime,
                NearestAppointmentPractitioner = GetPractitionerGeneralInfo(epicPractitioners, firstAppointment),
                FarthestAppointmentDate = lastAppointment.TimeSlot.StartTime
            };
        }

        private PatientAppointment[] BuildPatientAppointments(
            IEnumerable<Models.Appointment> ngdpAppointments,
            IReadOnlyDictionary<string, Models.PractitionerGeneralInfo> epicPractitioners)
        {
            var patientAppointments = ngdpAppointments
                .Select(appointment => new PatientAppointment
                {
                    Status = _mapper.Map<PatientAppointmentStatus>(appointment.Status),
                    StartTime = appointment.TimeSlot.StartTime,
                    Reason = appointment.Note,
                    IsTelehealth = appointment.IsTelehealthVisit,
                    Practitioner = GetPractitionerGeneralInfo(epicPractitioners, appointment),
                    MinutesDuration = appointment.TimeSlot.GetDurationInMinutes()
                })
                .ToArray();

            return patientAppointments;
        }

        private PatientGeneralInfo GetPatientGeneralInfo(
            IReadOnlyDictionary<string, Models.Patient> epicPatients,
            Models.Appointment appointment)
        {
            var patientGeneralInfo = appointment.IsProtectedByBtg
                ? GetBtgPatientGeneralInfo(appointment)
                : GetNonBtgPatientGeneralInfo(epicPatients, appointment);

            return patientGeneralInfo;
        }

        private PatientGeneralInfo GetBtgPatientGeneralInfo(Models.Appointment appointment)
        {
            var patientGeneralInfo = GetBasicPatientGeneralInfo(appointment);
            patientGeneralInfo.HumanName = BtgPatientHumanName;

            return patientGeneralInfo;
        }

        private PatientGeneralInfo GetNonBtgPatientGeneralInfo(
            IReadOnlyDictionary<string, Models.Patient> epicPatients,
            Models.Appointment appointment)
        {
            PatientGeneralInfo patientGeneralInfo;

            if (epicPatients.TryGetValue(appointment.Patient.Id, out var epicPatient))
            {
                patientGeneralInfo = _mapper.Map<PatientGeneralInfo>(epicPatient.PatientGeneralInfo);
            }
            else
            {
                _logger.LogWarning(
                    "Patient {PatientID} for appointment {AppointmentID} was not found in epic, using \"{PatientName}\" patient instead.",
                    appointment.Patient.Id,
                    appointment.Id,
                    UnknownPatientHumanName.FamilyName);

                patientGeneralInfo = GetBasicPatientGeneralInfo(appointment);
                patientGeneralInfo.HumanName = UnknownPatientHumanName;
            }

            return patientGeneralInfo;
        }

        private PatientGeneralInfo GetBasicPatientGeneralInfo(Models.Appointment appointment)
        {
            return new()
            {
                Id = appointment.Patient.Id
            };
        }

        private PractitionerGeneralInfo GetPractitionerGeneralInfo(
            IReadOnlyDictionary<string, Models.PractitionerGeneralInfo> epicPractitioners,
            Models.Appointment appointment)
        {
            PractitionerGeneralInfo practitionerGeneralInfo;

            if (epicPractitioners.TryGetValue(appointment.Practitioner.Id, out var epicPractitioner))
            {
                practitionerGeneralInfo = _mapper.Map<PractitionerGeneralInfo>(epicPractitioner);
            }
            else
            {
                _logger.LogWarning(
                    "Practitioner {PractitionerID} for appointment {AppointmentID} was not found in epic, using NGDP data instead.",
                    appointment.Practitioner.Id,
                    appointment.Id);

                practitionerGeneralInfo = new PractitionerGeneralInfo
                {
                    Id = appointment.Practitioner.Id,
                    HumanName = _mapper.Map<HumanName>(appointment.Practitioner.HumanName),
                    Role = PractitionerRole.Unknown
                };
            }

            return practitionerGeneralInfo;
        }

        private async Task<IReadOnlyDictionary<string, Models.Patient>> GetEpicPatientsAsync(IEnumerable<Models.Appointment> appointments)
        {
            var patientIds = appointments
                .Where(a => !a.IsProtectedByBtg)
                .Select(a => a.Patient.Id)
                .Distinct()
                .Select(id => id.ToIdWithExternalPrefix())
                .ToArray();

            if (!patientIds.Any())
            {
                return EmptyPatientDictionary;
            }

            var epicPatients = await _patientRepository.GetPatientsByIdsAsync(patientIds);

            var patientsDictionary = epicPatients.CollectEntitiesDictionary(IdentityHelper.ExternalIdPrefix);

            return patientsDictionary;
        }

        private async Task<IReadOnlyDictionary<string, Models.PractitionerGeneralInfo>> GetEpicPractitionersByIdentitiesAsync(
            IEnumerable<Models.Appointment> appointments)
        {
            var practitionerIds = appointments

                // There are some visit_prov_id values in NGDP datasource which correspond to non-human users (like "BL ORTHO US")
                // Epic does not return any information for such entities, so as a workaround we exclude practitioners without any given names from epic request
                // and use all the information we can get from NGDP to generate a response.
                .Where(a => a.Practitioner.HumanName.GivenNames?.Any() ?? false)
                .Select(a => a.Practitioner.Id)
                .Distinct()
                .Select(id => id.ToIdWithExternalPrefix())
                .ToArray();

            if (!practitionerIds.Any())
            {
                return EmptyPractitionerDictionary;
            }

            var epicPractitioners = await _practitionerRepository.GetPractitionersByIdsAsync(practitionerIds);

            var practitionersCollection = epicPractitioners.CollectEntitiesDictionary(IdentityHelper.ExternalIdPrefix);

            return practitionersCollection;
        }

        private Task<IReadOnlyDictionary<string, Models.PractitionerGeneralInfo>> GetEpicPractitionersAsync(params Models.Appointment[] appointments)
        {
            return GetEpicPractitionersByIdentitiesAsync(appointments);
        }
    }
}

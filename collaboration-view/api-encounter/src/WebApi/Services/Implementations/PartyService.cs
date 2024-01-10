// <copyright file="PartyService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Security.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberType = Duly.CollaborationView.Encounter.Api.Contracts.MemberType;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    internal class PartyService : IPartyService
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ICareTeamRepository _careTeamRepository;
        private readonly IMapper _mapper;
        private readonly IActiveDirectoryAccountIdentityService _activeDirectoryAccountIdentityService;

        public PartyService(
            IAppointmentService appointmentService,
            ICareTeamRepository careTeamRepository,
            IMapper mapper,
            IActiveDirectoryAccountIdentityService activeDirectoryAccountIdentityService)
        {
            _appointmentService = appointmentService;
            _careTeamRepository = careTeamRepository;
            _mapper = mapper;
            _activeDirectoryAccountIdentityService = activeDirectoryAccountIdentityService;
        }

        public async Task<IEnumerable<Party>> GetPartiesByPatientAndAppointmentIdAsync(string patientId, string appointmentId)
        {
            var activeUserTask = _activeDirectoryAccountIdentityService.GetUserAsync();
            var appointmentParticipantTask = _appointmentService.GetPractitionerByAppointmentIdAsync(appointmentId);
            var careTeamParticipantsTask = _careTeamRepository.GetCareTeamParticipantsByPatientIdAsync(patientId, CareTeamStatus.Active, CareTeamCategory.Longitudinal);

            await Task.WhenAll(activeUserTask, appointmentParticipantTask, careTeamParticipantsTask);

            var activeUser = _mapper.Map<Party>(activeUserTask.Result);
            var appointmentParty = _mapper.Map<Party>(appointmentParticipantTask.Result);
            var careTeamParties = _mapper.Map<IEnumerable<Party>>(careTeamParticipantsTask.Result);
            return SortUniqueParties(activeUser, new[] { appointmentParty }, careTeamParties);
        }

        private static IEnumerable<Party> SortUniqueParties(Party activeUser, IEnumerable<Party> appointmentParties, IEnumerable<Party> careTeamParties)
        {
            var combinedParties = UnionWithDistinct(appointmentParties, careTeamParties).ToArray();

            var doctorsOrderedByFamilyName = combinedParties.Where(x => x.MemberType == MemberType.Doctor)
                                .OrderBy(x => x.HumanName.FamilyName);

            var nonDoctorsOrderedByGivenNames = combinedParties.Where(x => x.MemberType != MemberType.Doctor)
                                .OrderBy(x => string.Join(string.Empty, x.HumanName.GivenNames));

            return CombineContent(activeUser, doctorsOrderedByFamilyName, nonDoctorsOrderedByGivenNames);
        }

        private static IEnumerable<Party> UnionWithDistinct(IEnumerable<Party> encounterParties, IEnumerable<Party> careTeamParties)
        {
            var keys = new HashSet<string>();
            foreach (var encounterParty in encounterParties)
            {
                keys.Add(encounterParty.Id);
                yield return encounterParty;
            }

            foreach (var careTeamParty in careTeamParties)
            {
                if (!keys.Contains(careTeamParty.Id))
                    yield return careTeamParty;
            }
        }

        private static IEnumerable<Party> CombineContent(
            Party activeUser,
            IEnumerable<Party> doctors,
            IEnumerable<Party> notDoctors)
        {
            var doctorsArray = doctors.ToArray();
            var notDoctorsArray = notDoctors.ToArray();

            var existingParty =
                doctorsArray.FindExistingParty(activeUser) ??
                notDoctorsArray.FindExistingParty(activeUser);

            activeUser.CopyProperties(existingParty);

            yield return activeUser;

            foreach (var doctor in doctorsArray)
            {
                if (existingParty != doctor)
                    yield return doctor;
            }

            foreach (var notDoctor in notDoctorsArray)
            {
                if (existingParty != notDoctor)
                    yield return notDoctor;
            }
        }
    }
}

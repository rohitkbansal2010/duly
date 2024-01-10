// <copyright file="MedicationService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IMedicationService"/>
    /// </summary>
    internal class MedicationService : IMedicationService
    {
        private static readonly Models.MedicationCategory[] DefaultCategories =
        {
            Models.MedicationCategory.Community,
            Models.MedicationCategory.PatientSpecified
        };
        private readonly IMedicationRepository _repository;
        private readonly IMapper _mapper;

        public MedicationService(
            IMapper mapper,
            IMedicationRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Medications> GetMedicationsByPatientIdAsync(string patientId)
        {
            var modelMedications = await _repository.GetMedicationsAsync(patientId, Models.MedicationStatus.Active, DefaultCategories);
            var medications = _mapper.Map<Medication[]>(modelMedications);

            return new Medications
            {
                Regular = medications
                    .Where(x => x.ScheduleType == MedicationScheduleType.Regular)
                    .OrderByDescending(x => x.StartDate)
                    .ThenBy(x => x.Title)
                    .ToArray(),

                Other = medications
                    .Where(x => x.ScheduleType == MedicationScheduleType.Other)
                    .OrderByDescending(x => x.StartDate)
                    .ThenBy(x => x.Title)
                    .ToArray()
            };
        }

        public async Task<Medications> GetMedicationsRequestByPatientIdAsync(string patientId, string appointmentId)
        {
            var modelMedications = await _repository.GetMedicationsRequestAsync(patientId, appointmentId, Models.MedicationStatus.Active, DefaultCategories);
            var medications = _mapper.Map<Medication[]>(modelMedications);

            return new Medications
            {
                Regular = medications
            .Where(x => x.ScheduleType == MedicationScheduleType.Regular)
            .OrderByDescending(x => x.StartDate)
            .ThenBy(x => x.Title)
            .ToArray(),

                Other = medications
            .Where(x => x.ScheduleType == MedicationScheduleType.Other)
            .OrderByDescending(x => x.StartDate)
            .ThenBy(x => x.Title)
            .ToArray()
            };
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="MedicationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IMedicationRepository"/>
    /// </summary>
    internal class MedicationRepository : IMedicationRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IMapper _mapper;
        private readonly IPatientsClient _patientsClient;
        private readonly IPatientIdClient _patientIdClient;

        public MedicationRepository(
            IEncounterContext encounterContext,
            IMapper mapper,
            IPatientsClient patientsClient,
            IPatientIdClient patientIdClient)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _patientsClient = patientsClient;
            _patientIdClient = patientIdClient;
        }

        public async Task<IEnumerable<Models.Medication>> GetMedicationsAsync(
            string patientId,
            Models.MedicationStatus medicationStatus,
            IEnumerable<Models.MedicationCategory> medicationCategories)
        {
            var epicMedicationStatus = _mapper.Map<MedicationStatus>(medicationStatus);
            var epicMedicationCategories = _mapper.Map<IEnumerable<MedicationCategory>>(medicationCategories);

            var medications =
                await _patientsClient.MedicationsAsync(
                    patientId,
                    _encounterContext.GetXCorrelationId(),
                    epicMedicationStatus,
                    epicMedicationCategories);

            return _mapper.Map<IEnumerable<Models.Medication>>(medications);
        }

        public async Task<IEnumerable<Models.Medication>> GetMedicationsRequestAsync(
        string patientId,
        string appointmentId,
        Models.MedicationStatus medicationStatus,
        IEnumerable<Models.MedicationCategory> medicationCategories)
        {
            var epicMedicationStatus = _mapper.Map<MedicationStatus>(medicationStatus);
            var epicMedicationCategories = _mapper.Map<IEnumerable<MedicationCategory>>(medicationCategories);

            var medications =
            await _patientIdClient.AppointmentIdAsync(
            patientId,
            appointmentId,
            _encounterContext.GetXCorrelationId(),
            epicMedicationStatus,
            epicMedicationCategories);

            return _mapper.Map<IEnumerable<Models.Medication>>(medications);
        }
    }
}

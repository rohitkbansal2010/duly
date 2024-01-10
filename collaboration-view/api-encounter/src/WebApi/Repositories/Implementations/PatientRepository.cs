// -----------------------------------------------------------------------
// <copyright file="PatientRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPatientRepository"/>
    /// </summary>
    internal class PatientRepository : IPatientRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IClient _client;
        private readonly IPatientsClient _patientsClient;
        private readonly IMapper _mapper;

        public PatientRepository(
            IEncounterContext encounterContext,
            IMapper mapper,
            IClient client,
            IPatientsClient patientsClient)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _client = client;
            _patientsClient = patientsClient;
        }

        public async Task<Models.Patient> GetPatientByIdAsync(string patientId)
        {
            var patient = await _client.PatientsGetAsync(patientId, _encounterContext.GetXCorrelationId());
            var patientInformation = _mapper.Map<Models.Patient>(patient);
            patientInformation.PatientAddress = _mapper.Map<List<Models.PatientAddress>>(patient.Address);
            patientInformation.PhoneNumber = _mapper.Map<List<Models.PhoneNumber>>(patient.PhoneNumber);
            return patientInformation;
        }

        public async Task<IEnumerable<Models.Patient>> GetPatientsByIdsAsync(string[] patientIds)
        {
            if (patientIds.Length == 0)
                return Array.Empty<Models.Patient>();

            var patients = await _client.PatientsGetAsync(patientIds, _encounterContext.GetXCorrelationId());
            return _mapper.Map<IEnumerable<Models.Patient>>(patients);
        }

        public async Task<List<Models.PatientPhoto>> GetPatientsPhotoByIdsAsync(Models.PatientPhotoByParam request)
        {
            var patientPhoto = _mapper.Map<PatientPhotoByParam>(request);
            var patientPhotoResponse = await _patientsClient.GetpatientphotobyidAsync(_encounterContext.GetXCorrelationId(), patientPhoto);
            var result = _mapper.Map<List<Models.PatientPhoto>>(patientPhotoResponse);
            return result;
        }
    }
}
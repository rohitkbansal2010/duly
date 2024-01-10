// <copyright file="PatientService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPatientService"/>
    /// </summary>
    internal class PatientService : IPatientService
    {
        private readonly IMapper _mapper;
        private readonly IPatientRepository _repository;

        public PatientService(
            IMapper mapper,
            IPatientRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Patient> GetPatientByIdAsync(string patientId)
        {
            var patient = await _repository.GetPatientByIdAsync(patientId);
            if (patient == null)
            { throw new System.NullReferenceException(); }

            var patientInformation = _mapper.Map<Patient>(patient);
            patientInformation.PhoneNumber = _mapper.Map<List<PhoneNumber>>(patient.PhoneNumber);

            var filterPatientId = patient.Identifiers.ToList().Find(s => s.StartsWith("EXTERNAL|")).Split("|")[1];
            var request = new Models.PatientPhotoByParam();
            request.PatientID = filterPatientId;
            request.PatientIDType = "EXTERNAL";
            var patientPhotos = new List<Models.PatientPhoto>();
            try
            {
                patientPhotos = await _repository.GetPatientsPhotoByIdsAsync(request);
            }
            catch
            {
                patientPhotos = null;
            }

            patientInformation.Photo = _mapper.Map<List<Attachment>>(patientPhotos);

            return patientInformation;
        }
    }
}
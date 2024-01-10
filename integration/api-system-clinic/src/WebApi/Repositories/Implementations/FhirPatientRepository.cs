// <copyright file="FhirPatientRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPatientRepository"/>
    /// </summary>
    internal class FhirPatientRepository : IPatientRepository
    {
        private readonly IPatientWithCompartmentsAdapter _adapter;
        private readonly IMapper _mapper;

        public FhirPatientRepository(
            IPatientWithCompartmentsAdapter adapter,
            IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<Patient> GetPatientByIdAsync(string patientId)
        {
            var fhirPatient = await _adapter.FindPatientByIdAsync(patientId);
            var systemPatient = _mapper.Map<Patient>(fhirPatient);
            return systemPatient;
        }

        public async Task<IEnumerable<Patient>> GetPatientsByIdentifiersAsync(string[] identifiers)
        {
            var fhirPatients = await _adapter.FindPatientsByIdentifiersAsync(identifiers);
            var systemPatients = _mapper.Map<IEnumerable<Patient>>(fhirPatients);
            return systemPatients;
        }

        public async Task<List<PatientPhoto>> GetPatientsPhotoByIdAsync(PatientPhotoByParam identifiers)
        {
            var request = _mapper.Map<Fhir.Adapter.Contracts.PatientPhotoByParam>(identifiers);
            var result = await _adapter.FindPatientPhotoByIdentifiersAsync(request);

            var listOfPatientPhoto = new List<PatientPhoto>();
            if (result != null)
            {
                foreach (var item in result.GetPatientPhotosResult.PatientPhotos)
                {
                    var patientPhoto = new PatientPhoto();
                    patientPhoto.Photo = Convert.ToBase64String(item.Data);
                    patientPhoto.FileExtension = item.FileExtension;
                    patientPhoto.FileSize = item.FileSize;
                    patientPhoto.Title = "Photo";

                    listOfPatientPhoto.Add(patientPhoto);
                }

                return listOfPatientPhoto;
            }

            return listOfPatientPhoto;
        }
    }
}
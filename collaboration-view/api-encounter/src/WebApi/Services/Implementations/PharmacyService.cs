// <copyright file="PharmacyService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPharmacyService"/>
    /// </summary>
    internal class PharmacyService : IPharmacyService
    {
        private readonly IMapper _mapper;
        private readonly IPharmacyRepository _repository;
        private readonly IPatientRepository _patientRepository;

        public PharmacyService(
            IMapper mapper,
            IPharmacyRepository repository,
            IPatientRepository patientRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _patientRepository = patientRepository;
        }

        public async Task<Pharmacy> GeTPreferredPharmacyByPatientIdAsync(string patientId)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(patientId);
            if (patient == null)
            { throw new System.NullReferenceException(); }
            var filterPateintId = patient.Identifiers.ToList().Find(s => s.StartsWith("EXTERNAL|")).Split("|")[1];
            var pharmacy = await _repository.GeTPreferredPharmacyByPatientIdAsync(filterPateintId);
            return _mapper.Map<Pharmacy>(pharmacy);
        }
    }
}

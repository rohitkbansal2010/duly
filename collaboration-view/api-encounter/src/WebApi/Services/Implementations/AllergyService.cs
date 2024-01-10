// <copyright file="AllergyService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IAllergyService"/>
    /// </summary>
    internal class AllergyService : IAllergyService
    {
        private readonly IMapper _mapper;
        private readonly IAllergyIntoleranceRepository _repository;

        public AllergyService(
            IMapper mapper,
            IAllergyIntoleranceRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<Allergy>> GetAllergiesForPatientAsync(string patientId)
        {
            var allergyIntolerances = await _repository.GetAllergyIntolerancesForPatientAsync(patientId, AllergyIntoleranceClinicalStatus.Active);
            var allergies = _mapper.Map<IEnumerable<Allergy>>(allergyIntolerances);
            return allergies.OrderByDescending(x => x.Recorded);
        }
    }
}

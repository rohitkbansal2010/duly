// <copyright file="FhirParticipantRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IParticipantRepository"/>
    /// </summary>
    internal class FhirParticipantRepository : IParticipantRepository
    {
        private readonly IEncounterWithCompartmentsAdapter _adapter;
        private readonly IMapper _mapper;

        public FhirParticipantRepository(
            IEncounterWithCompartmentsAdapter adapter,
            IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByEncounterIdAsync(string encounterId)
        {
            var fhirEncounterWithParticipants = await _adapter.FindEncounterWithParticipantsAsync(encounterId);
            var systemPractitioners = _mapper.Map<IEnumerable<PractitionerGeneralInfo>>(fhirEncounterWithParticipants.Practitioners);
            var systemParticipants = systemPractitioners.Select(practitioner => new Participant { Person = practitioner });
            return systemParticipants;
        }
    }
}

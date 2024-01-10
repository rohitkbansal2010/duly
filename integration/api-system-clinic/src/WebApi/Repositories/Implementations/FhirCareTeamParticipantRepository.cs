// <copyright file="FhirCareTeamParticipantRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces.Composite;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using Duly.Common.Infrastructure.Exceptions;
using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ICareTeamParticipantRepository"/>
    /// </summary>
    internal class FhirCareTeamParticipantRepository : ICareTeamParticipantRepository
    {
        private const string SystemValue = "http://loinc.org";
        private const string SystemCodeLongitude = "LA28865-6";
        private const string SystemCodeEpisode = "LA27977-0";

        private static readonly Coding _codingLongitude = new(SystemValue, SystemCodeLongitude);
        private static readonly Coding _codingEpisode = new(SystemValue, SystemCodeEpisode);

        private readonly ICareTeamWithCompartmentsAdapter _adapter;
        private readonly IGetFhirResourceById<R4.Encounter> _encounterById;
        private readonly IMapper _mapper;

        public FhirCareTeamParticipantRepository(
            ICareTeamWithCompartmentsAdapter adapter,
            IGetFhirResourceById<R4.Encounter> encounterById,
            IMapper mapper)
        {
            _adapter = adapter;
            _encounterById = encounterById;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CareTeamParticipant>> GetCareTeamsParticipantsByEncounterIdAsync(string encounterId, CareTeamStatus status, CareTeamCategory category)
        {
            var encounter = await _encounterById.GetFhirResourceByIdAsync(encounterId);
            if (encounter == null)
            {
                throw new EntityNotFoundException(nameof(Encounter), encounterId);
            }

            var endDateTime = encounter.Period?.StartElement?.Value == null
                ? DateTimeOffset.UnixEpoch : encounter.Period.StartElement.BuildDateTimeOffset();

            var searchCriteria = new CareTeamSearchCriteria
            {
                PatientReference = encounter.Subject.Reference,
                Status = status.ToString(),
                EndOfParticipation = endDateTime,
                CategoryCoding = BuildCoding(category)
            };

            return await CareTeamsParticipantsBySearchCriteria(searchCriteria);
        }

        public async Task<IEnumerable<CareTeamParticipant>> GetCareTeamsParticipantsByPatientIdAsync(string patientId, CareTeamStatus status, CareTeamCategory category)
        {
            var searchCriteria = new CareTeamSearchCriteria
            {
                PatientReference = patientId,
                Status = status.ToString(),
                EndOfParticipation = DateTimeOffset.UnixEpoch,
                CategoryCoding = BuildCoding(category)
            };

            return await CareTeamsParticipantsBySearchCriteria(searchCriteria);
        }

        private static Coding BuildCoding(CareTeamCategory category)
        {
            return category == CareTeamCategory.Longitudinal ? _codingLongitude : _codingEpisode;
        }

        private async Task<IEnumerable<CareTeamParticipant>> CareTeamsParticipantsBySearchCriteria(CareTeamSearchCriteria searchCriteria)
        {
            var careTeamWithParticipants = await _adapter.FindCareTeamsWithParticipantsAsync(searchCriteria);
            var practitioners = careTeamWithParticipants.SelectMany(participants => participants.Practitioners);
            var careTeamParticipants = _mapper.Map<IEnumerable<CareTeamParticipant>>(practitioners);
            return careTeamParticipants;
        }
    }
}
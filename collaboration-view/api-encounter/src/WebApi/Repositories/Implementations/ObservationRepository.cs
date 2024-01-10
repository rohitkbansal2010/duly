// <copyright file="ObservationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IObservationRepository"/>
    /// </summary>
    internal class ObservationRepository : IObservationRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IVitalSignsClient _vitalSignsClient;
        private readonly IObservationsClient _observationsClient;
        private readonly IMapper _mapper;

        public ObservationRepository(
            IEncounterContext encounterContext,
            IVitalSignsClient vitalSignsClient,
            IObservationsClient observationsClient,
            IMapper mapper)
        {
            _encounterContext = encounterContext;
            _vitalSignsClient = vitalSignsClient;
            _observationsClient = observationsClient;
            _mapper = mapper;
        }

        public Task<IEnumerable<Models.Observation>> GetObservationsForPatientAsync(string patientId, DateTimeOffset lowerBoundDate, DateTimeOffset upperBoundDate, Models.ObservationType[] observationTypes)
        {
            Task<ICollection<Observation>> GetObservations(ObservationType[] types) => _observationsClient.VitalSignsAsync(patientId, types, lowerBoundDate, upperBoundDate, _encounterContext.GetXCorrelationId());
            return FindObservationsForPatientAsync(observationTypes, GetObservations);
        }

        public Task<IEnumerable<Models.Observation>> GetLatestObservationsForPatientAsync(string patientId, Models.ObservationType[] observationTypes)
        {
            Task<ICollection<Observation>> GetObservations(ObservationType[] types) => _vitalSignsClient.LastAsync(patientId, types, _encounterContext.GetXCorrelationId());
            return FindObservationsForPatientAsync(observationTypes, GetObservations);
        }

        private async Task<IEnumerable<Models.Observation>> FindObservationsForPatientAsync(
            Models.ObservationType[] observationTypes,
            Func<ObservationType[], Task<ICollection<Observation>>> getObservations)
        {
            var types = _mapper.Map<ObservationType[]>(observationTypes);
            var encountersBySiteId = await getObservations(types);
            var result = _mapper.Map<Models.Observation[]>(encountersBySiteId);
            return result;
        }
    }
}
// <copyright file="FhirObservationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Api.Repositories.Mappings.ObservationConverters;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IObservationRepository"/>
    /// </summary>
    internal class FhirObservationRepository : IObservationRepository
    {
        private const int MinLowerBoundForYearFilterInYears = -3;
        private const int StepForYearFilterInMonths = -6;
        private const int InitialUpperBoundInDays = 1;

        private readonly IObservationWithCompartmentsAdapter _adapter;
        private readonly IMapper _mapper;
        private readonly IObservationEnricher _observationEnricher;

        public FhirObservationRepository(IObservationWithCompartmentsAdapter adapter, IMapper mapper, IObservationEnricher observationEnricher)
        {
            _adapter = adapter;
            _mapper = mapper;
            _observationEnricher = observationEnricher;
        }

        public async Task<IEnumerable<Observation>> FindLastObservationsForPatientAsync(string patientId, ObservationType[] observationTypes)
        {
            //TODO: pain level should be got by another way.
            observationTypes = observationTypes.Where(type => type != ObservationType.PainLevel).ToArray();

            var systemObservations = await FetchData(patientId, observationTypes);

            var lastObservations = systemObservations
                .GroupBy(x => x.Type)
                .Select(group => group.OrderByDescending(x => x.Date).First())
                .ToArray();

            _observationEnricher.EnrichResults(lastObservations, systemObservations);
            return lastObservations;
        }

        public async Task<IEnumerable<Observation>> FindObservationsForPatientAsync(string patientId, ObservationType[] observationTypes, DateTime lowerBoundDate, DateTime upperBoundBoundDate)
        {
            var searchCriteria = CreateSearchCriteria(patientId, observationTypes);
            searchCriteria.StartDateTime = lowerBoundDate;
            searchCriteria.EndDateTime = upperBoundBoundDate;

            var fhirObservations = await _adapter.FindObservationsWithCompartmentsAsync(searchCriteria);

            return _mapper.Map<IEnumerable<Observation>>(fhirObservations);
        }

        private static IEnumerable<string> MapObservationTypes(IEnumerable<ObservationType> observationTypes)
        {
            foreach (var observationType in observationTypes)
            {
                if (!observationType.ConvertFromObservationTypeToLoincCode(out var loincCodes))
                    continue;

                foreach (var loincCode in loincCodes)
                {
                    yield return loincCode;
                }
            }
        }

        private static ObservationSearchCriteria CreateSearchCriteria(string patientId, params ObservationType[] observationTypes)
        {
            return new ObservationSearchCriteria { PatientId = patientId, Category = "vital-signs", Codes = MapObservationTypes(observationTypes).ToArray() };
        }

        private static (DateTime LowerBoundDate, DateTime CurrentLowerBoundDate, DateTime CurrentUpperBoundBoundDate) BuildDateFilters()
        {
            var currentDate = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Unspecified);
            var lowerBoundDate = currentDate.AddYears(MinLowerBoundForYearFilterInYears);
            var (currentLowerBoundDate, currentUpperBoundBoundDate) = BuildNextStep(currentDate);
            return (lowerBoundDate, currentLowerBoundDate, currentUpperBoundBoundDate);
        }

        private static (DateTime CurrentLowerBoundDate, DateTime CurrentUpperBoundBoundDate) BuildNextStep(DateTime currentDate)
        {
            var currentUpperBoundBoundDate = currentDate.AddDays(InitialUpperBoundInDays);
            var currentLowerBoundDate = currentDate.AddMonths(StepForYearFilterInMonths);
            return (currentLowerBoundDate, currentUpperBoundBoundDate);
        }

        private async Task<List<Observation>> FetchData(string patientId, ObservationType[] observationTypes)
        {
            var (lowerBoundDate, currentLowerBoundDate, currentUpperBoundBoundDate) = BuildDateFilters();

            var systemObservations = new List<Observation>();
            while (currentLowerBoundDate >= lowerBoundDate)
            {
                systemObservations.AddRange(
                    await FindObservationsForPatientAsync(patientId, observationTypes, currentLowerBoundDate, currentUpperBoundBoundDate));

                observationTypes = observationTypes
                    .Except(systemObservations.Select(observation => observation.Type))
                    .ToArray();
                if (observationTypes.Length == 0)
                {
                    break;
                }

                (currentLowerBoundDate, currentUpperBoundBoundDate) = BuildNextStep(currentLowerBoundDate);
            }

            return systemObservations;
        }
    }
}

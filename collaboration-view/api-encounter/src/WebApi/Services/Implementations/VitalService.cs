// <copyright file="VitalService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IVitalService"/>
    /// </summary>
    internal class VitalService : IVitalService
    {
        /// <summary>
        /// The last three years.
        /// </summary>
        private const int YearsToAddToTheStartDate = 3;
        private const int DaysToAddToTheStartDate = 1;

        private static readonly ObservationType[] _observationTypes = (ObservationType[])Enum.GetValues(typeof(ObservationType));

        private readonly IObservationRepository _observationRepository;
        private readonly IMapper _mapper;

        public VitalService(
            IObservationRepository observationRepository,
            IMapper mapper)
        {
            _observationRepository = observationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VitalsCard>> GetLatestVitalsForPatientAsync(string patientId)
        {
            var observations = await _observationRepository.GetLatestObservationsForPatientAsync(patientId, _observationTypes);
            var result = _mapper.Map<IEnumerable<VitalsCard>>(observations);
            var filteredResult = result.FilterVitalCards();
            var completeList = filteredResult.AppendMissedVitalsCars();
            var sortedCompletedList = completeList.SortVitalsCard();
            return sortedCompletedList;
        }

        public async Task<VitalHistory> GetVitalHistoryForPatientByVitalsCardType(string patientId, DateTime startDate, VitalsCardType vitalsCardType)
        {
            var observationTypes = _mapper.Map<ObservationType[]>(vitalsCardType);
            var lowerBound = BuildLowerBound(startDate);
            var upperBound = BuildUpperBound(startDate);
            var observations = await _observationRepository.GetObservationsForPatientAsync(patientId, lowerBound, upperBound, observationTypes);
            var observationsArray = observations.ToArray();

            if (observationsArray.Length == 0)
            {
                return null;
            }

            return VitalHistoryBuilder.BuildVitalHistory(vitalsCardType, observationsArray);
        }

        private static DateTimeOffset BuildLowerBound(DateTime time)
        {
            var lowerBound = time.AddYears(-YearsToAddToTheStartDate);
            lowerBound = DateTime.SpecifyKind(lowerBound, DateTimeKind.Utc);
            return new DateTimeOffset(lowerBound, TimeSpan.Zero);
        }

        private static DateTimeOffset BuildUpperBound(DateTime time)
        {
            var upperBound = time.AddDays(DaysToAddToTheStartDate);
            upperBound = DateTime.SpecifyKind(upperBound, DateTimeKind.Utc);
            return new DateTimeOffset(upperBound, TimeSpan.Zero);
        }
    }
}
// <copyright file="ObservationLabResultComponentExtensions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Extensions
{
    internal static class ObservationLabResultComponentExtensions
    {
        private static readonly HashSet<string> ObservationInterpretationSubstantialCodes = new HashSet<string>
        {
            "D",
            "U",
            "<",
            ">",
            "A",
            "AA",
            "HH",
            "LL",
            "HU",
            "LU",
            "IND",
            "UNE"
        };

        public static bool CheckIfComponentIsAbnormal(this Models.ObservationLabResultComponent component)
        {
            var isAbnormal = component.Interpretations
                                 ?.Any(x => ObservationInterpretationSubstantialCodes.Contains(x.Code))
                             ?? false;

            if (isAbnormal)
            {
                return true;
            }

            return component.Measurements
                ?.Any(x =>
                    !CheckIfMeasurementIsInRange(x, component.ReferenceRange)) ?? false;
        }

        private static bool CheckIfMeasurementIsInRange(
            Models.ObservationLabResultMeasurement measurement,
            IEnumerable<Models.ObservationLabResultReferenceRange> ranges)
        {
            var measurementsWithUnits = ranges.Where(x =>
                measurement.Unit == x.Low.Unit &&
                measurement.Unit == x.High.Unit).ToArray();

            if (!measurementsWithUnits.Any())
            {
                return true;
            }

            return measurementsWithUnits.Any(x =>
                measurement.Value >= x.Low.Value &&
                measurement.Value <= x.High.Value);
        }
    }
}

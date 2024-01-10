// <copyright file="FhirDateTimeExtensions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Hl7.Fhir.Model;
using System;

namespace Duly.Clinic.Fhir.Adapter.Extensions.Fhir
{
    public static class FhirDateTimeExtensions
    {
        /// <summary>
        /// Builds DateTimeOffset from the FhirDateTime.
        /// </summary>
        /// <param name="source">FhirDateTime.</param>
        /// <returns>A DateTimeOffset.</returns>
        public static DateTimeOffset BuildDateTimeOffset(this FhirDateTime source)
        {
            // If a time zone is specified in this FhirDateTime, that zone is used,
            // otherwise, it is assumed to be UTC.
            if (source.TryToDateTimeOffset(out var result))
                return result;

            //Add time span to avoid switching between days.
            return source.ToDateTimeOffset(TimeSpan.Zero).AddHours(12);
        }
    }
}

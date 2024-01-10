// <copyright file="EncounterSearchCriteria.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Exceptions;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Rest;

namespace Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria
{
    /// <summary>
    /// Search criteria for Encounter.
    /// </summary>
    public class EncounterSearchCriteria : SearchCriteria
    {
        internal SearchParams ToSearchParams()
        {
            if (PatientId == null)
            {
                throw new MandatoryQueryParameterMissingException($"{nameof(PatientId)} is required");
            }

            var searchParams = new SearchParams();

            searchParams.ByPatientId(PatientId);

            if (StartDateTime.HasValue)
            {
                searchParams.ByGreaterOrEqualDate((System.DateTime)StartDateTime);
            }

            if (EndDateTime.HasValue)
            {
                if (EndDateTime < StartDateTime)
                {
                    throw new ContradictoryQueryException("Encounter query EndDateTime < StartDateTime");
                }

                searchParams.ByLessOrEqualDate((System.DateTime)EndDateTime);
            }

            return searchParams;
        }
    }
}
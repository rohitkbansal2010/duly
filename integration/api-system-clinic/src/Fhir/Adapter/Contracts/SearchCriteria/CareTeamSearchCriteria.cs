// <copyright file="CareTeamSearchCriteria.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Exceptions;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;

namespace Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria
{
    /// <summary>
    /// Search criteria for CareTeam.
    /// </summary>
    public class CareTeamSearchCriteria : SearchCriteria
    {
        /// <summary>
        /// Literal reference, Relative, internal or absolute URL.
        /// </summary>
        public string PatientReference { get; set; }

        /// <summary>
        /// The end time which CareTeam Participant is no longer a member.
        /// </summary>
        public DateTimeOffset EndOfParticipation { get; set; }

        /// <summary>
        /// Category of the CareTeam.
        /// </summary>
        public Coding CategoryCoding { get; set; }

        internal SearchParams ToSearchParams()
        {
            if (string.IsNullOrEmpty(PatientReference))
            {
                throw new MandatoryQueryParameterMissingException("PatientReference is missing.");
            }

            if (string.IsNullOrEmpty(Status))
            {
                throw new MandatoryQueryParameterMissingException("Status is missing.");
            }

            if (CategoryCoding == null)
            {
                throw new MandatoryQueryParameterMissingException("Coding is missing.");
            }

            if (EndOfParticipation == default)
            {
                throw new MandatoryQueryParameterMissingException("EndOfParticipation is missing.");
            }

            return new SearchParams().ByPatientReference(PatientReference).ByStatus(Status.ToLower());
        }
    }
}
// <copyright file="ServicerequestSearchCriteria.cs" company="Duly Health and Care">
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
    public class ServicerequestSearchCriteria : SearchCriteria
    {
        /// <summary>
        /// Literal reference, Relative, internal or absolute URL.
        /// </summary>
        public string PatientReference { get; set; }

        public string Category { get; set; }

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

            if (string.IsNullOrEmpty(Category))
            {
                throw new MandatoryQueryParameterMissingException("Category is missing.");
            }

            return new SearchParams().ByPatientReference(PatientReference).ByCategory(Category).ByStatus(Status.ToLower());
        }
    }
}
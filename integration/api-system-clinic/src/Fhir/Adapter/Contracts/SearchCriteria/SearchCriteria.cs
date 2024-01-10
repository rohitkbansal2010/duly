// <copyright file="SearchCriteria.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria
{
    /// <summary>
    /// Parameters for search.
    /// </summary>
    public class SearchCriteria
    {
        /// <summary>
        /// Reference Id of patient.
        /// </summary>
        public string PatientId { get; set; }

        /// <summary>
        /// Reference Id of Location.
        /// </summary>
        public string SiteId { get; set; }

        /// <summary>
        /// Start point in time.
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// End point in time.
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Status of resources. Possible values depend on resource for which this is used.
        /// </summary>
        public string Status { get; set; }
    }
}
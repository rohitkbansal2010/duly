// <copyright file="ImmunizationCdcCatalogs.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Represents the identifies for CDC Vaccine Administered (CVX) and CDC Vaccine Group catalogs.
    /// </summary>
    public class ImmunizationCdcCatalogs
    {
        /// <summary>
        /// Identify for CDC Vaccine Administered (CVX) catalog.
        /// </summary>
        public string CvxCatalogUID { get; set; }

        /// <summary>
        /// Identify for CDC Vaccine Group catalog.
        /// </summary>
        public string VaccineGroupCatalogUID { get; set; }
    }
}
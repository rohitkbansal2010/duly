// -----------------------------------------------------------------------
// <copyright file="CdcApiOptions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Configurations
{
    /// <summary>
    /// Configuration options for the CDC REST API.
    /// </summary>
    public class CdcApiOptions
    {
        /// <summary>
        /// The content model mnemonic.
        /// </summary>
        public string ContentModel { get; set; }

        /// <summary>
        /// The catalog mnemonic for CDC Vaccine Administered (CVX).
        /// </summary>
        public string CatalogCVX { get; set; }

        /// <summary>
        /// The catalog mnemonic for CDC Vaccine Group.
        /// </summary>
        public string CatalogVaccineGroup { get; set; }
    }
}
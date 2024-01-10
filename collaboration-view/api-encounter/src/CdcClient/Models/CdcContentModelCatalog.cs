// <copyright file="CdcContentModelCatalog.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.CdcClient.Models
{
    /// <summary>
    /// Catalog associated with a published content model.
    /// </summary>
    public class CdcContentModelCatalog
    {
        public string CatalogUID { get; set; }

        public string CatalogName { get; set; }

        public string CatalogVersion { get; set; }

        public string Mnemonic { get; set; }

        public string LastPublishedDate { get; set; }

        public string LastUpdatedDate { get; set; }

        public int? TermCount { get; set; }
    }
}

// <copyright file="CdcRelatedTermItem.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.CdcClient.Models
{
    public class CdcRelatedTermItem
    {
        public string RelationshipUID { get; set; }

        public string RelationshipMnemonic { get; set; }

        public string CatalogUID { get; set; }

        public string CatalogMnemonic { get; set; }

        public CdcTermSearchItem RelatedTerm { get; set; }
    }
}
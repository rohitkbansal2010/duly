// <copyright file="CdcCatalogSourceCodeRelations.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.CdcClient.Models
{
    /// <summary>
    /// Represents response with a collection of CdcRelatedTermItem objects for the TermUID or catalog/source codes requested.
    /// </summary>
    public class CdcCatalogSourceCodeRelations
    {
        public CdcTermRequestItem RequestedTerm { get; set; }

        public string[] Messages { get; set; }

        public CdcRelatedTermItem[] RelatedItems { get; set; }
    }
}

// <copyright file="CdcTermRequestItem.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.CdcClient.Models
{
    public class CdcTermRequestItem
    {
        public string CatalogIdentifier { get; set; }

        public string TermSourceCode { get; set; }

        public bool? IncludeDomainCharacteristics { get; set; }
    }
}
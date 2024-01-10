// -----------------------------------------------------------------------
// <copyright file="Provider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    public class Provider
    {
        [Description("Provider Id")]

        public int ID { get; set; }

        [Description("Provider LocationId")]

        public string LocationId { get; set; }

        [Description("Provider ProviderId")]

        public string ProviderId { get; set; }

        [Description("ProviderName")]

        public string ProviderName { get; set; }

        [Description("ProviderDisplayName")]

        public string ProviderDisplayName { get; set; }

        [Description("ProviderPhotoUR")]

        public string ProviderPhotoURL { get; set; }

        [Description("LocationLatitudeCoordinate")]

        public string LocationLatitudeCoordinate { get; set; }

        [Description("LocationLongitudeCoordinate")]

        public string LocationLongitudeCoordinate { get; set; }

        [Description("Provider City")]

        public string City { get; set; }

        [Description("ProviderSpecialty")]

        public string ProviderSpecialty { get; set; }

        [Description("Distance")]

        public double Distance { get; set; }

        [Description("LocationName")]

        public string LocationName { get; set; }

        [Description("LocationAdd_1")]

        public string LocationAdd_1 { get; set; }

        [Description("LocationState")]

        public string LocationState { get; set; }

        [Description("LocationZip")]

        public string LocationZip { get; set; }

        [Description("Department_Id")]

        public string Department_Id { get; set; }
    }
}

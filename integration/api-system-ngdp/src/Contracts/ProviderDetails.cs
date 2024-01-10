// <copyright file="ProviderDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Provider Details")]
    public class ProviderDetails
    {
        [Description("ID of Provider Table")]
        public int ID { get; set; }
        [Description("Id of the Location")]
        public string LocationId { get; set; }
        [Description("ID of the provider")]
        public string ProviderId { get; set; }
        [Description("Name of the Provider")]
        public string ProviderName { get; set; }
        [Description("Display name of the provider")]
        public string ProviderDisplayName { get; set; }
        [Description("Url of the providers photo")]
        public string ProviderPhotoURL { get; set; }
        [Description("Latitude coordinates of the location")]
        public string LocationLatitudeCoordinate { get; set; }
        [Description("Longitudnal coordinate of the location")]
        public string LocationLongitudeCoordinate { get; set; }
        [Description("city")]
        public string City { get; set; }
        [Description("Speciality of the provider")]
        public string ProviderSpecialty { get; set; }
        [Description("Distance")]
        public string Distance { get; set; }
        [Description("Name of the location")]
        public string LocationName { get; set; }
        [Description("Address of the location")]
        public string LocationAdd_1 { get; set; }
        [Description("State of the location")]
        public string LocationState { get; set; }
        [Description("Zipcode of the location")]
        public string LocationZip { get; set; }
    }
}
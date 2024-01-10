// <copyright file="LabMockData.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts.MockData
{
    public class LabMockData
    {
        public ProviderDetail ProviderDetails { get; set; }
        public string[] Tests { get; set; }
    }

    public class ProviderDetail
    {
        public string Id { get; set; }
        public HumanName HumanName { get; set; }
        public Photo Photo { get; set; }
        public string Specialty { get; set; }
        public Locations Location { get; set; }
    }

    public class Locations
    {
        public string Id { get; set; }
        public Addres Address { get; set; }
        public GeographicCoordinate GeographicCoordinates { get; set; }
        public string PhoneNumber { get; set; }
        public double Distance { get; set; }
    }

    public class Addres
    {
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipeCode { get; set; }
    }

    public class GeographicCoordinate
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}

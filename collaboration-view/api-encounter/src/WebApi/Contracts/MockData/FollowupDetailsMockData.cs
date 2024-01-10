// -----------------------------------------------------------------------
// <copyright file="FollowupDetailsMockData.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts.MockData
{
    public class FollowupDetailsMockData
    {
        public string Id { get; set; }
        public HumanName HumanName { get; set; }
        public Photo Photo { get; set; }

        public string Specialty { get; set; }

        public Location Location { get; set; }
    }

    public class Photo
    {
        public string ContentType { get; set; }
        public string Url { get; set; }
    }

    public class Location
    {
        public string Id { get; set; }
        public Address Address { get; set; }
        public GeographicCoordinates GeographicCoordinates { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkingHours { get; set; }
        public double Distance { get; set; }
    }

    public class Address
    {
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipeCode { get; set; }
    }

    public class GeographicCoordinates
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}

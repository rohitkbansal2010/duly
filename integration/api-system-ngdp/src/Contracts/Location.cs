// <copyright file="Location.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Ngdp.Contracts
{
    [Description("Details and position information for a physical place")]
    public class Location
    {
        [Description("Unique code or number identifying the location to its users")]
        public Identifier Identifier { get; set; }

        [Description("Name of the location as used by humans")]
        public string Name { get; set; }

        [Description("Physical location")]
        public Address Address { get; set; }

        [Description("Distance between the location and patient's home ")]
        public double? DistanceFromPatientHome { get; set; }
    }
}
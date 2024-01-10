// <copyright file="ImagingAddressPart.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    public class ImagingAddressPart
    {
        [Description("Street number")]
        public string Street_number { get; set; }
        [Description("Imaging address number")]
        public string Number { get; set; }

        [Description("Address")]
        public string Address { get; set; }
        [Description("City")]
        public string City { get; set; }
        [Description("Postal Code")]
        public string Postcode { get; set; }
        [Description("County")]
        public string County { get; set; }
        [Description("State")]
        public string State { get; set; }
        [Description("Country")]
        public string Country { get; set; }
        [Description("Planet")]
        public string Planet { get; set; }
        [Description("System")]
        public string System { get; set; }
        [Description("Arm")]
        public string Arm { get; set; }
        [Description("Galaxy")]
        public string Galaxy { get; set; }
        [Description("Group")]
        public string Group { get; set; }
        [Description("Cluster")]
        public string Cluster { get; set; }
        [Description("Super Cluster")]
        public string Supercluster { get; set; }
    }
}

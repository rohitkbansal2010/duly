// <copyright file="ImagingAddress.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    public class ImagingAddress
    {
        [Description("Latitude of location")]
        public double Lat { get; set; }
        [Description("Longitude of location")]
        public double Lng { get; set; }
        [Description("Imaging address")]
        public string Address { get; set; }
        [Description("Imaging address detail")]
        public ImagingAddressPart Parts { get; set; }
    }
}

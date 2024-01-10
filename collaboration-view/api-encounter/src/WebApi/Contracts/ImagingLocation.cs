// -----------------------------------------------------------------------
// <copyright file="ImagingLocation.cs" company="Duly Health and Care">
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
    public class ImagingLocation
    {
        [Description("Imaging Location Id")]
        public int Id { get; set; }
        [Description("Imaging Title")]
        public string Title { get; set; }
        [Description("Imaging date created")]
        public DateTime DateCreated { get; set; }
        [Description("Imaging date updated")]
        public DateTime DateUpdated { get; set; }
        [Description("Imaging address")]
        public ImagingAddress Address { get; set; }
        [Description("Imaging provider ids")]
        public List<DeptProvider> ProviderIds { get; set; }
        [Description("Distance")]
        public double Distance { get; set; }
    }

    public class DeptProvider
    {
        public string Dept_id { get; set; }
        public List<string> Provider_ids { get; set; }
    }
}

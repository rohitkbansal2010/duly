// <copyright file="ImagingScheduleDate.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    public class ImagingScheduleDate
    {
        [Description("Provider id")]
        public string ProviderId { get; set; }

        [Description("Schedule date")]
        public IEnumerable<ScheduleDate> ScheduleDates { get; set; }
    }
}

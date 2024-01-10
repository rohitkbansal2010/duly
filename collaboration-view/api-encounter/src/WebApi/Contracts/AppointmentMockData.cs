// -----------------------------------------------------------------------
// <copyright file="AppointmentMockData.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    public class AppointmentMockData
    {
        [Description("ProviderType for this Appointment")]
        public string ProviderType { get; set; }
    }
}

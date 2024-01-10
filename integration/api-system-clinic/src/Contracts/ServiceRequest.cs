// <copyright file="ServiceRequest.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Data of the test orders")]
    public class ServiceRequest
    {
        [Description("Names of the lab orders")]
        public List<Orders> TestOrder { get; set; }

        [Description("Count of Lab orders")]
        public int? OrderCount { get; set; }
    }
}
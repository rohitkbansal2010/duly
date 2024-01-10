// <copyright file="UpdatePatientTargetsResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class UpdatePatientTargetsResponse
    {
        [Description("Patient Target Identifier")]
        public long PatientTargetId { get; set; }

        [Description("HTTP Status Code")]
        public string StatusCode { get; set; }

        [Description("Message")]
        public string Message { get; set; }
    }
}

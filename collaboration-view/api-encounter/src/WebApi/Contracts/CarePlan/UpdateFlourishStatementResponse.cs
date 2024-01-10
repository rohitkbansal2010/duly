// <copyright file="UpdateFlourishStatementResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class UpdateFlourishStatementResponse
    {
        [Description("Patiend Plan Identifier")]
        public long PatientPlanId { get; set; }
        [Description("Status Code")]
        public int StatusCode { get; set; }
        [Description("Message")]
        public string Message { get; set; }
    }
}
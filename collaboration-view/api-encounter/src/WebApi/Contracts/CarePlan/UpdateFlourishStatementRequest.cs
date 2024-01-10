// <copyright file="UpdateFlourishStatementRequest.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class UpdateFlourishStatementRequest
    {
        [Description("Patiend Plan Identifier")]
        public long PatientPlanId { get; set; }
        [Description("Flourish Statement")]
        public string FlourishStatement { get; set; }
    }
}
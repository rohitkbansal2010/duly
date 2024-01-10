// <copyright file="UpdatePatientTarget.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class UpdatePatientTarget
    {
        [Description("IsReviewed")]
        public bool IsReviewed { get; set; }
    }
}

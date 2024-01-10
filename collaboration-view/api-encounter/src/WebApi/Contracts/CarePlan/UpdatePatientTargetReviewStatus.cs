// <copyright file="UpdatePatientTargetReviewStatus.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class UpdatePatientTargetReviewStatus
    {
        [Description("IsReviewed")]
        public bool IsReviewed { get; set; }
    }
}

// <copyright file="PatientConditionsDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class PatientConditionsDetails
    {
        [Description("PatientConditionId")]
        public long PatientConditionId { get; set; }
        [Description("ConditionDisplayName")]
        public string ConditionDisplayName { get; set; }
        [Description("PatientTargetDetails")]
        public PatientTargetDetails PatientTargetDetails { get; set; }
    }
}
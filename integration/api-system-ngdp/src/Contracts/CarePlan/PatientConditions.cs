// <copyright file="PatientConditions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class PatientConditions
    {
        [Description("PatientPlanId")]
        public long PatientPlanId { get; set; }
        [Description("ConditionId")]
        public long[] ConditionId { get; set; }
    }
}

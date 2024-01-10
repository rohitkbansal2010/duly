// <copyright file="PatientConditionsDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Patient Condition Details.
    /// </summary>
    public class PatientConditionsDetails
    {
        /// <summary>
        /// Patient Plan Id.
        /// </summary>
        public long PatientPlanId { get; set; }
        /// <summary>
        /// Condition Id.
        /// </summary>
        public long[] ConditionId { get; set; }
        /// <summary>
        /// Condition Details.
        /// </summary>
        public ConditionDetails ConditionDetails { get; set; }
    }
}
// <copyright file="ConditionDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Condition Details.
    /// </summary>
    public class ConditionDetails
    {
        /// <summary>
        /// Condition Id.
        /// </summary>
        public long ConditionId { get; set; }
        /// <summary>
        /// Condition Short Name.
        /// </summary>
        public string ConditionShortName { get; set; }
        /// <summary>
        /// Patient Target Details.
        /// </summary>
        public PatientTargetDetails PatientTargetDetails { get; set; }
    }
}
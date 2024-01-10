// <copyright file="GetPatientConditions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Patient Condition Details.
    /// </summary>
    public class GetPatientConditions
    {
        /// <summary>
        /// Patient Condition Id.
        /// </summary>
        public long PatientConditionId { get; set; }

        /// <summary>
        /// Condition Id.
        /// </summary>
        public long ConditionId { get; set; }

        /// <summary>
        /// Condition Short Name.
        /// </summary>
        public string ConditionShortName { get; set; }

        /// <summary>
        /// Condition Display Name.
        /// </summary>
        public string ConditionDisplayName { get; set; }
    }
}
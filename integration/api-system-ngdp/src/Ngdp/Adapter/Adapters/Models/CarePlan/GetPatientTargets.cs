// <copyright file="GetPatientTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Patient Target Details.
    /// </summary>
    public class GetPatientTargets
    {
        /// <summary>
        /// Patient Target Identifier.
        /// </summary>
        public long PatientTargetId { get; set; }

        /// <summary>
        /// Target Identifier.
        /// </summary>
        public long TargetId { get; set; }

        /// <summary>
        /// Target Name.
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// Patient Condition Identifier.
        /// </summary>
        public long PatientConditionId { get; set; }
    }
}
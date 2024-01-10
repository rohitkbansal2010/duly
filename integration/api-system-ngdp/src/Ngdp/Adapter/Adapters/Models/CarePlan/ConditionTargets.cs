// <copyright file="ConditionTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Condition targets of condition.
    /// </summary>
    public class ConditionTargets
    {
        /// <summary>
        /// Target Id of condition targets.
        /// </summary>
        public long TargetId { get; set; }

        /// <summary>
        /// Condition Id of condition targets.
        /// </summary>
        public string ConditionId { get; set; }

        /// <summary>
        /// Target Name of condition targets.
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// Min value of condition targets.
        /// </summary>
        public string MinValue { get; set; }

        /// <summary>
        /// Max value of condition targets.
        /// </summary>
        public string MaxValue { get; set; }

        /// <summary>
        /// Description of condition targets.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Measurement Value.
        /// </summary>
        public string MeasurementValue { get; set; }

        /// <summary>
        /// Measurement Unit.
        /// </summary>
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Active.
        /// </summary>
        public bool Active { get; set; }
    }
}
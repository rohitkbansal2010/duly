﻿// <copyright file="PatientTarget.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Condition.
    /// </summary>
    public class PatientTarget
    {
        /// <summary>
        /// Patient target id.
        /// </summary>
        public long PatientTargetId { get; set; }

        /// <summary>
        /// Patient plan id.
        /// </summary>
        public long PatientPlanId { get; set; }

        /// <summary>
        /// Condition id.
        /// </summary>
        public long ConditionId { get; set; }

        /// <summary>
        /// Target type.
        /// </summary>
        public string TargetType { get; set; }

        /// <summary>
        /// Target id.
        /// </summary>
        public long TargetId { get; set; }

        /// <summary>
        /// Custom target id.
        /// </summary>
        public long CustomTargetId { get; set; }

        /// <summary>
        /// Min value.
        /// </summary>
        public string MinValue { get; set; }

        /// <summary>
        /// Max value.
        /// </summary>
        public string MaxValue { get; set; }

        /// <summary>
        /// Measurement unit.
        /// </summary>
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Created by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Updated by.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Target value.
        /// </summary>
        public int TargetValue { get; set; }

        /// <summary>
        /// Base value.
        /// </summary>
        public string BaseValue { get; set; }

        /// <summary>
        /// Recent Value.
        /// </summary>
        public int RecentValue { get; set; }
    }
}
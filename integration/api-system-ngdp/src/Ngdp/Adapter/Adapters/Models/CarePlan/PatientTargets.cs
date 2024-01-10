// <copyright file="PatientTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Condition.
    /// </summary>
    public class PatientTargets
    {
        /// <summary>
        /// Patient condition id.
        /// </summary>
        public long PatientConditionId { get; set; }

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
        /// Max value.
        /// </summary>
        public string MeasurementValue { get; set; }

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
    }
}
// <copyright file="GetPatientTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Patient Targets.
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
        /// Min value.
        /// </summary>
        public string MinValue { get; set; }

        /// <summary>
        /// Max value.
        /// </summary>
        public string MaxValue { get; set; }

        /// <summary>
        /// Normal Value.
        /// </summary>
        public string NormalValue { get; set; }

        /// <summary>
        /// Base Value.
        /// </summary>
        public string BaseValue { get; set; }

        /// <summary>
        /// Target Value.
        /// </summary>
        public string TargetValue { get; set; }

        /// <summary>
        /// Recent Value.
        /// </summary>
        public string RecentValue { get; set; }

        /// <summary>
        /// Measurement unit.
        /// </summary>
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Patient Condition Identifier.
        /// </summary>
        public string PatientConditionId { get; set; }

        /// <summary>
        /// Condition id.
        /// </summary>
        public long ConditionId { get; set; }

        /// <summary>
        /// Review Status.
        /// </summary>
        public int IsReviewed { get; set; }
    }
}
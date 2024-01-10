// <copyright file="UpdatePatientTargets.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// PatientTarget.
    /// </summary>
    public class UpdatePatientTargets
    {
        /// <summary>
        /// Patient Plan Identifier.
        /// </summary>
        public long PatientPlanId { get; set; }

        /// <summary>
        /// Condition ids.
        /// </summary>
        public IEnumerable<long> ConditionIds { get; set; }

        /// <summary>
        /// Target Identifier.
        /// </summary>
        public long TargetId { get; set; }

        /// <summary>
        /// Target Name.
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// Minimum value for target.
        /// </summary>
        public string MinValue { get; set; }

        /// <summary>
        /// Maximum value for target.
        /// </summary>
        public string MaxValue { get; set; }

        /// <summary>
        /// Baseline Value for target.
        /// </summary>
        public string BaseValue { get; set; }

        /// <summary>
        /// PatientTarget.
        /// </summary>
        public string TargetValue { get; set; }

        /// <summary>
        /// Recent value.
        /// </summary>
        public string RecentValue { get; set; }

        /// <summary>
        /// Measurement unit.
        /// </summary>
        public string MeasurementUnit { get; set; }
    }
}

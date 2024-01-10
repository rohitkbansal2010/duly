// <copyright file="Condition.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Condition for a specific patient.
    /// </summary>
    internal class Condition
    {
        /// <summary>
        /// Condition Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of a condition.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Status of Condition.
        /// </summary>
        public ConditionClinicalStatus ClinicalStatus { get; set; }

        /// <summary>
        /// Date condition was recorded.
        /// </summary>
        public DateTimeOffset RecordedDate { get; set; }

        /// <summary>
        /// Date condition was resolved.
        /// </summary>
        public Period AbatementPeriod { get; set; }
    }
}

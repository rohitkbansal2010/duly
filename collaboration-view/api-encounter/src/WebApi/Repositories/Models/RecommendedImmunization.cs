// <copyright file="RecommendedImmunization.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Record of patient's recommended immunization.
    /// </summary>
    internal class RecommendedImmunization
    {
        /// <summary>
        /// Patient who has the immunization.
        /// </summary>
        public RecommendedImmunizationPatient Patient { get; set; }

        /// <summary>
        /// The name of the vaccine.
        /// </summary>
        public string VaccineName { get; set; }

        /// <summary>
        /// Due date.
        /// </summary>
        public DateTimeOffset DueDate { get; set; }

        /// <summary>
        /// Recommended immunization status.
        /// </summary>
        public RecommendedImmunizationStatus Status { get; set; }
    }
}
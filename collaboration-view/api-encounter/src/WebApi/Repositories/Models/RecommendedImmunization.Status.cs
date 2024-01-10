// <copyright file="RecommendedImmunization.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Recommended immunization status codes.
    /// </summary>
    internal enum RecommendedImmunizationStatus
    {
        /// <summary>
        /// The patient is not due for their next vaccination.
        /// </summary>
        NotDue,

        /// <summary>
        /// The patient is due soon for their next vaccination.
        /// </summary>
        DueSoon,

        /// <summary>
        /// The patient is due for their next vaccination.
        /// </summary>
        DueOn,

        /// <summary>
        /// The patient is considered overdue for their next vaccination.
        /// </summary>
        Overdue,

        /// <summary>
        /// The patient is considered postpone for their next vaccination.
        /// </summary>
        Postponed
    }
}
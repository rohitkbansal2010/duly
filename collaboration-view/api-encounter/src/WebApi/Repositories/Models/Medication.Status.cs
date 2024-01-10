// <copyright file="Medication.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Medication Status Codes.
    /// </summary>
    internal enum MedicationStatus
    {
        /// <summary>
        /// The medication is available for use.
        /// </summary>
        Active,

        /// <summary>
        /// The medication is not available for use.
        /// </summary>
        Inactive,

        /// <summary>
        /// The medication was entered in error.
        /// </summary>
        EnteredInError
    }
}
// <copyright file="MedicationCategory.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Type of medication usage.
    /// </summary>
    internal enum MedicationCategory
    {
        /// <summary>
        /// Medications administered or consumed in inpatient setting.
        /// </summary>
        Inpatient,

        /// <summary>
        /// Medications administered or consumed in outpatient setting (clinics, doctor's office, etc.).
        /// </summary>
        Outpatient,

        /// <summary>
        /// Medications administered or consumed by the patient in their home.
        /// </summary>
        Community,

        /// <summary>
        /// Medications in use, specified by patient.
        /// </summary>
        PatientSpecified
    }
}

// <copyright file="Medication.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Record of medication being taken by a patient.
    /// </summary>
    internal class Medication
    {
        /// <summary>
        /// The identifier for this medication.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Drug information.
        /// </summary>
        public Drug Drug { get; set; }

        /// <summary>
        /// Medication status.
        /// </summary>
        public MedicationStatus Status { get; set; }

        /// <summary>
        /// Reason for why the medication is being/was taken.
        /// </summary>
        public MedicationReason Reason { get; set; }

        /// <summary>
        /// The interval when the medication is/was/will be taken.
        /// </summary>
        public Period Period { get; set; }

        /// <summary>
        /// Person that provided the information about the taking of this medication.
        /// </summary>
        public PractitionerGeneralInfo Prescriber { get; set; }

        /// <summary>
        /// Details of how medication is/was taken or should be taken.
        /// </summary>
        public MedicationDosage[] Dosages { get; set; }
    }
}

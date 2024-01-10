// <copyright file="Medication.Dosage.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// How the medication is/was taken or should be taken.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Models")]
    internal class MedicationDosage
    {
        /// <summary>
        /// True if taken as needed (PRN). False otherwise.
        /// </summary>
        public bool AsNeeded { get; set; }

        /// <summary>
        /// Amount of medication per dose. The amount of therapeutic or other substance given at one administration event.
        /// </summary>
        public Quantity DoseQuantity { get; set; }

        /// <summary>
        /// The patient instructions for the prescription.
        /// </summary>
        public string PatientInstruction { get; set; }

        /// <summary>
        /// The patient instructions for the prescription.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The frequency.
        /// </summary>
        public Timing Timing { get; set; }
    }
}
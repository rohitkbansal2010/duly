// <copyright file="Medication.Category.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Medication Categories")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Contract")]
    public enum MedicationCategory
    {
        [Description("Medications administered in inpatient setting.")]
        Inpatient,

        [Description("Medications administrated in Clinic like IV, Syringe, etc.")]
        Outpatient,

        [Description("Prescriptions.")]
        Community,

        [Description("Specified by patient.")]
        PatientSpecified
    }
}
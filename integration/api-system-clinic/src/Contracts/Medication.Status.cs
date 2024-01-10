// <copyright file="Medication.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Medication Status Codes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Contract")]
    public enum MedicationStatus
    {
        [Description("The medication is available for use.")]
        Active,

        [Description("The medication is not available for use.")]
        Inactive,

        [Description("The medication was entered in error.")]
        EnteredInError
    }
}
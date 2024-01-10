// <copyright file="Immunization.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Immunization Status Codes")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Contract")]
    public enum ImmunizationStatus
    {
        [Description("The immunization was completed.")]
        Completed,

        [Description("The immunization was not done.")]
        NotDone,

        [Description("The immunization was entered in error.")]
        EnteredInError
    }
}
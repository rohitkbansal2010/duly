// <copyright file="Observation.ComponentType.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Types of observation component measurements")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Contract")]
    public enum ObservationComponentType
    {
        [Description("Systolic component of blood pressure")]
        Systolic,
        [Description("Diastolic component of blood pressure")]
        Diastolic,
        [Description("Weight component of BMI")]
        Weight,
        [Description("Height component of BMI")]
        Height
    }
}

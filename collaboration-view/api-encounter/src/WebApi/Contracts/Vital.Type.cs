// -----------------------------------------------------------------------
// <copyright file="Vital.Type.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Types of vitals")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1602:Enumeration items should be documented",
        Justification = "Contract")]
    public enum VitalType
    {
        [Description("Blood Pressure data Systolic and Diastolic")]
        BloodPressure,

        [Description("Level of oxygen in blood")]
        BloodOxygen,

        [Description("Heart rate")]
        HeartRate,

        [Description("Respiratory rate")]
        RespiratoryRate,

        [Description("Pain level")]
        PainLevel,

        [Description("Temperature")]
        Temperature,

        [Description("Weight")]
        Weight,

        [Description("Height")]
        Height,

        [Description("BMI")]
        BodyMassIndex
    }
}
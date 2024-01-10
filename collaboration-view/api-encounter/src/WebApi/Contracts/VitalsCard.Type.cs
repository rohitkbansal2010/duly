// -----------------------------------------------------------------------
// <copyright file="VitalsCard.Type.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Types of vitals cards")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1602:Enumeration items should be documented",
        Justification = "Contract")]
    public enum VitalsCardType
    {
        [Description("Blood Pressure data Systolic and Diastolic")]
        BloodPressure,

        [Description("Level of oxygen in blood")]
        BloodOxygen,

        [Description("Heart rate")]
        HeartRate,

        [Description("Temperature")]
        Temperature,

        [Description("Respiratory rate")]
        RespiratoryRate,

        [Description("Pain level")]
        PainLevel,

        [Description("Weight and height")]
        WeightAndHeight,

        [Description("BMI")]
        BodyMassIndex
    }
}
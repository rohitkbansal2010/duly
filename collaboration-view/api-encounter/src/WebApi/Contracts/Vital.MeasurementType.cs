// -----------------------------------------------------------------------
// <copyright file="Vital.MeasurementType.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Types of vital measurements")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1602:Enumeration items should be documented",
        Justification = "Contract")]
    public enum VitalMeasurementType
    {
        [Description("Systolic blood pressure")]
        SystolicBloodPressure,

        [Description("Diastolic blood pressure")]
        DiastolicBloodPressure,

        [Description("Oxygen saturation")]
        OxygenSaturation,

        [Description("Heart rate")]
        HeartRate,

        [Description("Respiratory rate")]
        RespiratoryRate,

        [Description("Pain level")]
        PainLevel,

        [Description("Body temperature")]
        BodyTemperature,

        [Description("Body weight")]
        BodyWeight,

        [Description("Body height")]
        BodyHeight,

        [Description("BMI")]
        BodyMassIndex
    }
}
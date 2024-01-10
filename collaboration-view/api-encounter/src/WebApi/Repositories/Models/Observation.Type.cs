// <copyright file="Observation.Type.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Types of observation.
    /// </summary>
    internal enum ObservationType
    {
        /// <summary>
        /// Blood pressure.
        /// </summary>
        BloodPressure,

        /// <summary>
        /// Oxygen saturation.
        /// </summary>
        OxygenSaturation,

        /// <summary>
        /// Heart rate.
        /// </summary>
        HeartRate,

        /// <summary>
        /// Respiratory rate.
        /// </summary>
        RespiratoryRate,

        /// <summary>
        /// Pain level.
        /// </summary>
        PainLevel,

        /// <summary>
        /// Body temperature.
        /// </summary>
        BodyTemperature,

        /// <summary>
        /// Body weight.
        /// </summary>
        BodyWeight,

        /// <summary>
        /// Body height.
        /// </summary>
        BodyHeight,

        /// <summary>
        /// BMI - BodyMassIndex.
        /// </summary>
        BodyMassIndex
    }
}

// <copyright file="Observation.ComponentType.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Types of observation component measurements.
    /// </summary>
    internal enum ObservationComponentType
    {
        /// <summary>
        /// Systolic component of blood pressure.
        /// </summary>
        Systolic,

        /// <summary>
        /// Diastolic component of blood pressure.
        /// </summary>
        Diastolic,

        /// <summary>
        /// Weight component of BMI.
        /// </summary>
        Weight,

        /// <summary>
        /// Height component of BMI.
        /// </summary>
        Height
    }
}

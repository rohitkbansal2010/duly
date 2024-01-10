// -----------------------------------------------------------------------
// <copyright file="UiConfiguration.Type.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.UiConfig.Adapter.Contracts
{
    /// <summary>
    /// User interface configuration types.
    /// </summary>
    public enum UiConfigurationType
    {
        /// <summary>
        /// Global user interface configuration.
        /// </summary>
        GlobalConfiguration,

        /// <summary>
        /// User interface configuration for the Clinic.
        /// </summary>
        ClinicConfiguration,

        /// <summary>
        /// User interface configuration for the Patient.
        /// </summary>
        PatientConfiguration
    }
}
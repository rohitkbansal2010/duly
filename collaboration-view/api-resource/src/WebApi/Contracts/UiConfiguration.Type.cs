// -----------------------------------------------------------------------
// <copyright file="UiConfiguration.Type.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Resource.Api.Contracts
{
    [Description("User interface configuration types.")]
    public enum UiConfigurationType
    {
        [Description("Global user interface configuration.")]
        GlobalConfiguration,

        [Description("User interface configuration for the Clinic.")]
        ClinicConfiguration,

        [Description("User interface configuration for the Patient.")]
        PatientConfiguration
    }
}
// -----------------------------------------------------------------------
// <copyright file="Appointment.Type.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Describe a type of an appointment.
    /// </summary>
    internal enum AppointmentType
    {
        /// <summary>
        /// On-site appointment.
        /// </summary>
        OnSite,

        /// <summary>
        /// Telehealth Appointment.
        /// </summary>
        Telehealth
    }
}

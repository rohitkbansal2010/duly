// -----------------------------------------------------------------------
// <copyright file="UiConfiguration.ApplicationPart.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.UiConfig.Adapter.Contracts
{
    /// <summary>
    /// Logical parts of the 'Collaboration View' application.
    /// </summary>
    public enum ApplicationPart
    {
        /// <summary>
        /// The application shows a calendar with appointments.
        /// </summary>
        Calendar,

        /// <summary>
        /// The application shows the appropriate views for the selected appointment.
        /// </summary>
        CurrentAppointment
    }
}
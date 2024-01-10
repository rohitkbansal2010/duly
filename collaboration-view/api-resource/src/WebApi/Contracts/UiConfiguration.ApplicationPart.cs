// -----------------------------------------------------------------------
// <copyright file="UiConfiguration.ApplicationPart.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Resource.Api.Contracts
{
    [Description("Logical parts of the 'Collaboration View' application")]
    public enum ApplicationPart
    {
        [Description("The application shows a calendar with appointments.")]
        Calendar,

        [Description("The application shows the appropriate views for the selected appointment.")]
        CurrentAppointment
    }
}
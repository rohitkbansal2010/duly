// <copyright file="Appointment.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("The free/busy status of an appointment.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Enum in contracts")]
    public enum AppointmentStatus
    {
        [Description("The Appointment has not yet started.")]
        Planned,

        [Description("The Patient is present for the appointment, however is not currently meeting with a practitioner.")]
        Arrived,

        [Description("The appointment has begun and the patient is present / the practitioner and the patient are meeting.")]
        InProgress,

        [Description("The appointment has ended.")]
        Completed,

        [Description("This is the only currently returned status, because the datasource does not contain real-time appointments, therefore we cannot rely on the status it provides.")]
        Unknown
    }
}
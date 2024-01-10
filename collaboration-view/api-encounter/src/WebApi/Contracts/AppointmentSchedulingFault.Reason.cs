// <copyright file="AppointmentSchedulingFault.Reason.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Reason for the appointment scheduling failure.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Contract")]
    public enum AppointmentSchedulingFaultReason
    {
        [Description("Appointment scheduling failed due to the timeslot being already taken.")]
        TimeSlotNotAvailable,

        [Description("Appointment scheduling failed due to other reason.")]
        Other
    }
}
// <copyright file="Appointment.Status.Param.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Ngdp.Contracts
{
    [Description("The status of an appointment to be used for queries.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Enum in contracts")]
    public enum AppointmentStatusParam
    {
        [Description("The patient is present for the appointment, however is not currently meeting with a practitioner.")]
        Arrived,

        [Description("The appointment has ended before it has begun.")]
        Canceled,

        [Description("The appointment has ended.")]
        Completed,

        [Description("No additional description is available.")]
        LeftWithoutSeen,

        [Description("The patient has not arrived for the appointment.")]
        NoShow,

        [Description("The appointment is scheduled.")]
        Scheduled,

        [Description("The appointment status is unresolved in the datasource.")]
        Unresolved,

        [Description("No additional description is available.")]
        ChargeEntered
    }
}

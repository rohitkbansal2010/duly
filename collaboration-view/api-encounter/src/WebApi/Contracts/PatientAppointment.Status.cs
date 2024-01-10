// <copyright file="PatientAppointment.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("The status of a patient's appointment.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Contract")]
    public enum PatientAppointmentStatus
    {
        [Description("The appointment is confirmed to go ahead at the date/times specified.")]
        Scheduled,

        [Description("The appointment has been cancelled.")]
        Cancelled,

        [Description("Some or all of the participant(s) have not/did not appear for the appointment (usually the patient).")]
        NoShow,

        [Description("The appointment has completed.")]
        Completed,

        [Description("Database returned unexpected status.")]
        Unknown
    }
}
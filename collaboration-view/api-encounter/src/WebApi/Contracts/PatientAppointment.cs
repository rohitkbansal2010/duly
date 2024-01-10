// -----------------------------------------------------------------------
// <copyright file="PatientAppointment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about the patient's appointment.")]
    public class PatientAppointment
    {
        [Description("Participant of the appointment from practitioner side.")]
        [Required]
        public PractitionerGeneralInfo Practitioner { get; set; }

        [Description("Whether this appointment is telehealth appointment.")]
        [Required]
        public bool IsTelehealth { get; set; }

        [Description("Reason of the appointment.")]
        public string Reason { get; set; }

        [Description("Status of the appointment")]
        [Required]
        public PatientAppointmentStatus Status { get; set; }

        [Description("When the appointment should start.")]
        [Required]
        public DateTimeOffset StartTime { get; set; }

        [Description("The estimated length of the appointment.")]
        public int? MinutesDuration { get; set; }
    }
}

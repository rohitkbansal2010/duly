// <copyright file="Appointment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Security.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("An interaction during which services are provided to the patient")]
    public class Appointment
    {
        [Description("Identifier by which this appointment is known")]
        [Required]
        [Identity]
        public string Id { get; set; }

        [Description("Visit information")]
        [Required]
        public AppointmentVisit Visit { get; set; }

        [Description("Status of the visit")]
        [Required]
        public AppointmentStatus Status { get; set; }

        [Description("When appointment is to take place")]
        [Required]
        public TimeSlot TimeSlot { get; set; }

        [Description("Participant of the meeting from patient side")]
        [Required]
        public Patient Patient { get; set; }

        [Description("Participant of the meeting from practitioner side")]
        [Required]
        public Practitioner Practitioner { get; set; }

        [Description("Whether this appointment needs Break the Glass flow to retrieve all the data")]
        [Required]
        public bool IsProtectedByBtg { get; set; }

        [Description("Reason for visit")]
        public string Note { get; set; }

        [Description("Whether this visit is telehealth appointment")]
        public bool IsTelehealthVisit { get; set; }
    }
}

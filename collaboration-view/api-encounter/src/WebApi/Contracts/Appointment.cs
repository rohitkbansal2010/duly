// -----------------------------------------------------------------------
// <copyright file="Appointment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("A booking of a healthcare event among patient(s), practitioner(s) for a specific date/time")]
    public class Appointment
    {
        [Description("Ids for this Appointment")]
        [Required]
        public string Id { get; set; }

        [Description("Shown on a subject line in a meeting request, or appointment list")]
        [Required]
        public string Title { get; set; }

        [Description("Type of the visit")]
        public AppointmentType Type { get; set; }

        [Description("Status of the visit")]
        public AppointmentStatus Status { get; set; }

        [Description("When appointment is to take place")]
        [Required]
        public TimeSlot TimeSlot { get; set; }

        [Description("Where appointment is to take place")]
        public Location Location { get; set; }

        [Description("Participant of the meeting from patient side")]
        [Required]
        public PatientExtendedInfo Patient { get; set; }

        [Description("Participant of the meeting from practitioner side")]
        [Required]
        public PractitionerGeneralInfo Practitioner { get; set; }

        [Description("The appointment is subject to break-the-glass restrictions, patient info is hidden")]
        [Required]
        public bool IsProtectedByBtg { get; set; }
    }
}

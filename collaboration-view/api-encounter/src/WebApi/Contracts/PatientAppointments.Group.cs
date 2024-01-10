// -----------------------------------------------------------------------
// <copyright file="PatientAppointments.Group.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about the patient's appointments with the same appointment type.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    public class PatientAppointmentsGroup
    {
        [Description("The title of the appointment type used for the grouping.")]
        [Required]
        public string Title { get; set; }

        [Description("Appointments with the same appointment type.")]
        [Required]
        public PatientAppointment[] Appointments { get; set; }

        [Description("Participant of the nearest meeting from practitioner side.")]
        [Required]
        public PractitionerGeneralInfo NearestAppointmentPractitioner { get; set; }

        [Description("Date of the nearest appointment in the group.")]
        [Required]
        public DateTimeOffset NearestAppointmentDate { get; set; }

        [Description("Date of the farthest appointment in the group. If there is more than one appointment in the group.")]
        public DateTimeOffset? FarthestAppointmentDate { get; set; }
    }
}

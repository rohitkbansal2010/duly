// -----------------------------------------------------------------------
// <copyright file="Appointment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// An interaction during which services are provided to the patient.
    /// </summary>
    internal class Appointment
    {
        /// <summary>
        /// Identifier by which this appointment is known.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Visit information.
        /// </summary>
        public AppointmentVisit Visit { get; set; }

        /// <summary>
        /// Status of the visit.
        /// </summary>
        public AppointmentStatus Status { get; set; }

        /// <summary>
        /// When appointment is to take place.
        /// </summary>
        public TimeSlot TimeSlot { get; set; }

        /// <summary>
        /// Participant of the meeting from patient side.
        /// </summary>
        public AppointmentPatient Patient { get; set; }

        /// <summary>
        /// Participant of the meeting from practitioner side.
        /// </summary>
        public AppointmentPractitioner Practitioner { get; set; }

        /// <summary>
        /// Reason of the visit.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// The appointment is subject to break-the-glass restrictions.
        /// </summary>
        public bool IsProtectedByBtg { get; set; }

        /// <summary>
        /// Whether this visit is telehealth appointment.
        /// </summary>
        public bool IsTelehealthVisit { get; set; }
    }
}
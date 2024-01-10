// <copyright file="PatientPlan.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    public class PatientPlan
    {
        /// <summary>
        /// PatiendId  of patient.
        /// </summary>
        public string PatientId { get; set; }

        /// <summary>
        /// ProviderId  of patient.
        /// </summary>
        public string ProviderId { get; set; }

        /// <summary>
        /// Flourishing Statement.
        /// </summary>
        public string FlourishingStatement { get; set; }

        /// <summary>
        /// Appointment Id.
        /// </summary>
        public string AppointmentId { get; set; }
    }
}
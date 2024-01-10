// <copyright file="PatientPlan.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// plan of Patients.
    /// </summary>
    public class PatientPlan
    {
        /// <summary>
        /// Flourishing Statement.
        /// </summary>
        public string FlourishingStatement { get; set; }

        /// <summary>
        /// AppointmentId  of patient.
        /// </summary>
        public string AppointmentId { get; set; }

        /// <summary>
        /// PatiendId  of patient.
        /// </summary>
        public string PatientId { get; set; }

        /// <summary>
        /// ProviderId  of patient.
        /// </summary>
        public string ProviderId { get; set; }
    }
}

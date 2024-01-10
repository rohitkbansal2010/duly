// <copyright file="PatientPlanDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// plan of Patients.
    /// </summary>
    public class PatientPlanDetails
    {
        /// <summary>
        /// Appointment Identifier.
        /// </summary>
        public string AppointmentId { get; set; }

        /// <summary>
        /// Patient Plan Name.
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// Patient Plan Identifier.
        /// </summary>
        public long PatientPlanId { get; set; }

        /// <summary>
        /// Patient Life Goals.
        /// </summary>
        public PatientLifeGoalsDetails PatientLifeGoalsDetails { get; set; }

        /// <summary>
        /// Patient Conditions Details.
        /// </summary>
        public PatientConditionsDetails PatientConditionsDetails { get; set; }
    }
}

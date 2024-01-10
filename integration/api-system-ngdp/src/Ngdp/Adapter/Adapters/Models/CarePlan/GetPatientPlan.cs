// <copyright file="GetPatientPlan.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// plan of Patients.
    /// </summary>
    public class GetPatientPlan
    {
        /// <summary>
        /// Patient Plan Identifier.
        /// </summary>
        public long PatientPlanId { get; set; }

        /// <summary>
        /// Patient Plan Name.
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// Flourish Statement.
        /// </summary>
        public string FlourishStatement { get; set; }
    }
}

// <copyright file="PatientConditions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// conditions of Patients.
    /// </summary>
    public class PatientConditions
    {
        /// <summary>
        /// PatiendPlanId  of patient.
        /// </summary>
        public long PatientPlanId { get; set; }

        /// <summary>
        /// ConditionId  of patient.
        /// </summary>
        public long[] ConditionId { get; set; }
    }
}

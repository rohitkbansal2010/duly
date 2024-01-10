// <copyright file="CarePlanDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
     /// <summary>
     /// Care Plan Details.
     /// </summary>
    public class CarePlanDetails
    {
        /// <summary>
        /// Appointment Identifier.
        /// </summary>
        public string AppointmentId { get; set; }

        /// <summary>
        /// PlanName.
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// ConditionDisplayName.
        /// </summary>
        public string ConditionDisplayName { get; set; }

        /// <summary>
        /// TargetName.
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// ActionName.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// CustomTarget.
        /// </summary>
        public string CustomTarget { get; set; }

        /// <summary>
        /// CustomAction.
        /// </summary>
        public string CustomAction { get; set; }

        /// <summary>
        /// PatientPlanId.
        /// </summary>
        public long PatientPlanId { get; set; }

        /// <summary>
        /// PatientConditionId.
        /// </summary>
        public long PatientConditionId { get; set; }

        /// <summary>
        /// PatientTargetId.
        /// </summary>
        public long PatientTargetId { get; set; }

        /// <summary>
        /// PatientActionId.
        /// </summary>
        public long PatientActionId { get; set; }

        /// <summary>
        /// CustomTargetId.
        /// </summary>
        public long CustomTargetId { get; set; }

        /// <summary>
        /// CustomActionId.
        /// </summary>
        public long CustomActionId { get; set; }

        /// <summary>
        /// CustomActionId.
        /// </summary>
        public long PatientLifeGoalId { get; set; }

        /// <summary>
        /// CustomActionId.
        /// </summary>
        public string LifeGoalName { get; set; }

        /// <summary>
        /// LifeGoalDescription.
        /// </summary>
        public string LifeGoalDescription { get; set; }

    }
}
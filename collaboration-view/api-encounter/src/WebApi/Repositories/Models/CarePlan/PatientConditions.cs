// <copyright file="PatientConditions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    public class PatientConditions
    {
        /// <summary>
        /// PatiendPlanId  of patient.
        /// </summary>
        public long PatientPlanId { get; set; }

        /// <summary>
        /// Condition Ids to be added.
        /// </summary>
        public long[] AddConditionIds { get; set; }

        /// <summary>
        /// Condition Ids to be removed.
        /// </summary>
        public long[] RemoveConditionIds { get; set; }
    }
}
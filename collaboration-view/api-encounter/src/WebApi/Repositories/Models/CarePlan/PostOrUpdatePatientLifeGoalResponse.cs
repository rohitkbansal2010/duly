// <copyright file="PostOrUpdatePatientLifeGoalResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    public class PostOrUpdatePatientLifeGoalResponse
    {
        /// <summary>
        /// Patient Plan Id.
        /// </summary>
        public long PatientPlanId { get; set; }

        /// <summary>
        /// Patient Life Goals Updated Or Created.
        /// </summary>
        public IEnumerable<Models.CarePlan.PatientLifeGoal> PatientLifeGoals { get; set; }

        /// <summary>
        /// Status Code.
        /// </summary>
        public string DeletedLifeGoalIds { get; set; }
    }
}
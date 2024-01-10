// <copyright file="PostRequestForLifeGoals.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PostRequestForLifeGoals
    {
        [Description("Patient Plan Id")]
        public long PatientPlanId { get; set; }

        [Description("Patient Life Goals")]
        public IEnumerable<PatientLifeGoal> PatientLifeGoal { get; set; }

        [Description("Patient Life Goals to be removed")]
        public IEnumerable<long> DeletedLifeGoalIds { get; set; }
    }
}
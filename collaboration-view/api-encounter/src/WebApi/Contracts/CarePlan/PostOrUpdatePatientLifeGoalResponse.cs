// <copyright file="PostOrUpdatePatientLifeGoalResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PostOrUpdatePatientLifeGoalResponse
    {
        [Description("Patient Life Goals Created or Updated")]
        public IEnumerable<PatientLifeGoal> PatientLifeGoals { get; set; }

        [Description("Identifier of Deleted Life Goals")]
        public string DeletedLifeGoalIds { get; set; }

        [Description("Status Code")]
        public string StatusCode { get; set; }

        [Description("Message")]
        public string Message { get; set; }
    }
}

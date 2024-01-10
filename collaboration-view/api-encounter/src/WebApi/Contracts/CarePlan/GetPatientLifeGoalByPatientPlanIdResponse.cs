// <copyright file="GetPatientLifeGoalByPatientPlanIdResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetPatientLifeGoalByPatientPlanIdResponse
    {
        public IEnumerable<GetPatientLifeGoalByPatientPlanId> PatientLifeGoals { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
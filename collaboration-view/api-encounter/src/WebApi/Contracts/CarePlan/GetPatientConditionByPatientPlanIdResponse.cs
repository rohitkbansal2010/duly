// <copyright file="GetPatientConditionByPatientPlanIdResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetPatientConditionByPatientPlanIdResponse
    {
        public IEnumerable<GetPatientConditionByPatientPlanId> Conditions { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
// <copyright file="PostPatientPlanResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PostPatientPlanResponse
    {
        public long PatientPlanId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
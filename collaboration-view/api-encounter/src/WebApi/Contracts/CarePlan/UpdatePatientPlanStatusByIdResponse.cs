// <copyright file="UpdatePatientPlanStatusByIdResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class UpdatePatientPlanStatusByIdResponse
    {
        public long PatientPlanId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsCompleted { get; set; }
    }
}
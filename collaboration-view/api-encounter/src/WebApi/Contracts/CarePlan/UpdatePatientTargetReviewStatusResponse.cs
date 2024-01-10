// <copyright file="UpdatePatientTargetReviewStatusResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class UpdatePatientTargetReviewStatusResponse
    {
        public long PatientTargetId { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
    }
}

// <copyright file="DeletePatientTargetResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class DeletePatientTargetResponse
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsDeleted { get; set; }
    }
}
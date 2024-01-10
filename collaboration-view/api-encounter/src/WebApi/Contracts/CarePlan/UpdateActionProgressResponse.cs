// <copyright file="UpdateActionProgressResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class UpdateActionProgressResponse
    {
        public long PatientActionId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}

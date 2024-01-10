// <copyright file="PostCustomActionsResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PostCustomActionsResponse
    {
        public long CustomActionId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
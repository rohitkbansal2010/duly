// <copyright file="PostCustomTargetsResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PostCustomTargetsResponse
    {
        public long CustomTargetId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
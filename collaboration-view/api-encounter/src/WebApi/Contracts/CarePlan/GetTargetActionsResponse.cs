// <copyright file="GetTargetActionsResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetTargetActionsResponse
    {
        public long Id { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
    }
}
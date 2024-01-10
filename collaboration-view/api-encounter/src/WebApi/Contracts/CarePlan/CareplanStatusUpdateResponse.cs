// <copyright file="CareplanStatusUpdateResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class CareplanStatusUpdateResponse
    {
        public long RecordID { get; set; }

        public string Message { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsCompleted { get; set; }
    }
}
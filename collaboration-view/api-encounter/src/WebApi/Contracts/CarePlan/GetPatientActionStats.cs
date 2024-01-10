// <copyright file="GetPatientActionStats.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetPatientActionStats
    {
        [Description("Total Number of Patient Actions")]
        public long TotalNumberOfPatientActions { get; set; }

        [Description("Number of Distinct Actions")]
        public long NumberOfDistinctActions { get; set; }

        [Description("Number of Actions Completed")]
        public long NumberOfActionsCompleted { get; set; }
    }
}
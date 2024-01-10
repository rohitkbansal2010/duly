// <copyright file="GetPatientActionStats.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Actions of Targets.
    /// </summary>
    public class GetPatientActionStats
    {
        /// <summary>
        /// Total Number of Patient Actions.
        /// </summary>
        public long TotalNumberOfPatientActions { get; set; }

        /// <summary>
        /// Number of Distinct Actions for the patient plan.
        /// </summary>
        public long NumberOfDistinctActions { get; set; }

        /// <summary>
        /// Number of Patient Actions Completed.
        /// </summary>
        public long NumberOfActionsCompleted { get; set; }
    }
}
// <copyright file="PatientLifeGoalTargetMapping.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Text.Json.Serialization;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// PatientLifeGoalTarget.
    /// </summary>
    public class PatientLifeGoalTargetMapping
    {
        /// <summary>
        /// Patient Life Goal Id.
        /// </summary>
        public long PatientLifeGoalId { get; set; }

        /// <summary>
        /// Patient Target Id.
        /// </summary>
        public long PatientTargetId { get; set; }
    }
}

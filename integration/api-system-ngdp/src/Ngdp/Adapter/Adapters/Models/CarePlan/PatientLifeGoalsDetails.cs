// <copyright file="PatientLifeGoalsDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Life Goals.
    /// </summary>
    public class PatientLifeGoalsDetails
    {
        /// <summary>
        /// Patient Life Goal Identifier.
        /// </summary>
        public long PatientLifeGoalId { get; set; }

        /// <summary>
        /// Life Goal Name.
        /// </summary>
        public string LifeGoalName { get; set; }

        /// <summary>
        /// Life Goal Description.
        /// </summary>
        public string LifeGoalDescription { get; set; }
    }
}

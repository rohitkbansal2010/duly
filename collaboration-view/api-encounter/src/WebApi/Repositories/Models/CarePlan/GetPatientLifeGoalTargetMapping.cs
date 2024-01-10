// <copyright file="GetPatientLifeGoalTargetMapping.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    public class GetPatientLifeGoalTargetMapping
    {
        /// <summary>
        /// Patient Life Goal Id.
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

        /// <summary>
        /// Life Goal Category.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Priority of Life Goal.
        /// </summary>
        public int Priority { get; set; }

    }
}

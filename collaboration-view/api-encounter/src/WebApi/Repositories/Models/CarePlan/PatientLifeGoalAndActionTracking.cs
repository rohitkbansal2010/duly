// <copyright file="PatientLifeGoalAndActionTracking.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    public class PatientLifeGoalAndActionTracking
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

        /// <summary>
        /// Patient Target Identifier.
        /// </summary>
        public long PatientTargetId { get; set; }

        /// <summary>
        /// Target identifier.
        /// </summary>
        public long TargetId { get; set; }

        /// <summary>
        /// Target Name.
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// Patient Action Identifier.
        /// </summary>
        public long PatientActionId { get; set; }

        /// <summary>
        /// Action Identifier.
        /// </summary>
        public long ActionId { get; set; }

        /// <summary>
        /// Custom Action Identifier.
        /// </summary>
        public long CustomActionId { get; set; }

        /// <summary>
        /// Action Name.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Action Progress.
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Notes.
        /// </summary>
        public string Notes { get; set; }
    }
}
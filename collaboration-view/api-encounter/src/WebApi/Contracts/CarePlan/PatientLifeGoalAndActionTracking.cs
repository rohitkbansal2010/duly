// <copyright file="PatientLifeGoalAndActionTracking.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PatientLifeGoalAndActionTracking
    {
        [Description("Patient Life Goal Id")]
        public long PatientLifeGoalId { get; set; }

        [Description("Life Goal Name")]
        public string LifeGoalName { get; set; }

        [Description("Life Goal Description")]
        public string LifeGoalDescription { get; set; }

        [Description("Life Goal Category")]
        public string CategoryName { get; set; }

        [Description("Priority of Life Goal")]
        public int Priority { get; set; }

        [Description("Patient Target Identifier")]
        public long PatientTargetId { get; set; }

        [Description("Target identifier")]
        public long TargetId { get; set; }

        [Description("Target Name")]
        public string TargetName { get; set; }

        [Description("Patient Action Identifier")]
        public long PatientActionId { get; set; }

        [Description("Action Identifier")]
        public long ActionId { get; set; }

        [Description("Custom Action Identifier")]
        public long CustomActionId { get; set; }

        [Description("Action Name")]
        public string ActionName { get; set; }

        [Description("Description")]
        public string Description { get; set; }

        [Description("Action Progress")]
        public int Progress { get; set; }

        [Description("Notes")]
        public string Notes { get; set; }
    }
}
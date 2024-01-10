// <copyright file="GetPatientLifeGoalAndActionTrackingResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetPatientLifeGoalAndActionTrackingResponse
    {
        public GetPatientLifeGoalAndActionTrackingResponse()
        {
            MyActions = new List<MyActions>();
            OtherActions = new OtherActions();
        }

        [Description("Life Goals And the actions mapped with them")]
        public List<MyActions> MyActions { get; set; }

        [Description("Other Actions which are not mapped to any life goals")]
        public OtherActions OtherActions { get; set; }
    }

    public class MyActions
    {
        public MyActions()
        {
            PatientTargets = new List<PatientTarget>();
            PatientActions = new List<PatientAction>();
        }
        [Description("Patient Life Goal Id")]
        public long PatientLifeGoalId { get; set; }

        [Description("Life Goal Name")]
        public string LifeGoalName { get; set; }

        [Description("Life Goal Description")]
        public string LifeGoalDescription { get; set; }

        [Description("Life Goal Category")]
        public string CategoryName { get; set; }

        [Description("Priority Life Goal")]
        public int Priority { get; set; }

        [Description("List of Targets")]
        public List<PatientTarget> PatientTargets { get; set; }

        [Description("List of Actions")]
        public List<PatientAction> PatientActions { get; set; }
    }

    public class OtherActions
    {
        public OtherActions()
        {
            PatientTargets = new List<PatientTarget>();
            PatientActions = new List<PatientAction>();
        }

        [Description("Patient Targets")]
        public List<PatientTarget> PatientTargets { get; set; }

        [Description("Patient Actions")]
        public List<PatientAction> PatientActions { get; set; }
    }

    public class PatientAction
    {
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

    public class PatientTarget
    {
        [Description("Patient Target Identifier")]
        public long PatientTargetId { get; set; }

        [Description("Target Identifier")]
        public long TargetId { get; set; }

        [Description("Target Name")]
        public string TargetName { get; set; }
    }
}
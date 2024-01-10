// <copyright file="PatientTargetDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PatientTargetDetails
    {
        [Description("PatientTargetId")]
        public long PatientTargetId { get; set; }
        [Description("TargetName")]
        public string TargetName { get; set; }
        [Description("CustomTargetId")]
        public long CustomTargetId { get; set; }
        [Description("CustomTarget")]
        public string CustomTarget { get; set; }
        [Description("PatientActionDetails")]
        public PatientActionDetails PatientActionDetails { get; set; }
    }
}
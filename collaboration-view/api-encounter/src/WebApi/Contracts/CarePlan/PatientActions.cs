// <copyright file="PatientActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PatientActions
    {
        [Description("Patiend Action Id")]
        public long PatientActionId { get; set; }
        [Description("Patiend Target Id")]
        public long PatientTargetId { get; set; }
        [Description("Action Id")]
        public long ActionId { get; set; }
        [Description("Custom Action Id")]
        public long CustomActionId { get; set; }
        [Description("Deleted")]
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
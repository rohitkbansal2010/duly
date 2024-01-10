// <copyright file="PatientActionDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class PatientActionDetails
    {
        [Description("PatientActionId")]
        public long PatientActionId { get; set; }
        [Description("ActionName")]
        public string ActionName { get; set; }

        [Description("CustomAction")]
        public string CustomAction { get; set; }
        [Description("CustomActionId")]
        public long CustomActionId { get; set; }
    }
}
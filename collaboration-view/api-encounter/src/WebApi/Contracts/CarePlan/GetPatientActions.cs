// <copyright file="GetPatientActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetPatientActions
    {
        [Description("PatientActionId")]
        public long PatientActionId { get; set; }

        [Description("ActionId")]
        public long ActionId { get; set; }

        [Description("CustomActionId")]
        public long CustomActionId { get; set; }

        [Description("ActionName")]
        public string ActionName { get; set; }

        [Description("Action Description")]
        public string Description { get; set; }

        [Description("IsSelected")]
        public bool IsSelected { get; set; }
    }
}
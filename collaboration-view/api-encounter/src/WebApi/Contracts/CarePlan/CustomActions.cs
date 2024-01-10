// <copyright file="CustomActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class CustomActions
    {
        [Description("Patient Target ID")]
        public long PatientTargetId { get; set; }

        [Description("Action Name")]
        public string ActionName { get; set; }

        [Description("Description")]
        public string Description { get; set; }

        [Description("Is Selected")]
        public bool IsSelected { get; set; }
    }
}
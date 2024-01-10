// <copyright file="TargetActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class TargetActions
    {
        [Description("Action Id")]
        public long ActionId { get; set; }

        [Description("Action Name")]
        public string ActionName { get; set; }

        [Description("Action Description")]
        public string Description { get; set; }
    }
}
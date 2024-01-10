// <copyright file="TargetActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class TargetActions
    {
        [Description("Action Id")]
        public long ActionId { get; set; }

        [Description("Target Id")]
        public long TargetId { get; set; }

        [Description("Action Name")]
        public string ActionName { get; set; }
        [Description("Action Description")]
        public string Description { get; set; }

        [Description("Active")]
        public bool Active { get; set; }

        [Description("Notes")]
        public string Notes { get; set; }
    }
}
// <copyright file="Condition.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class Condition
    {
        [Description("Condition Id")]
        public long ConditionId { get; set; }

        [Description("Condition Short Name")]
        public string ConditionShortName { get; set; }

        [Description("Condition Display Name")]
        public string ConditionDisplayName { get; set; }

        [Description("Active")]
        public bool Active { get; set; }
    }
}
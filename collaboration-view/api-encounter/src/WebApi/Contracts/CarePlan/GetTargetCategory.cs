// <copyright file="GetTargetCategory.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts.CarePlan
{
    public class GetTargetCategory
    {
        [Description("Category Name")]
        public string CategoryName { get; set; }
        [Description("Category Value")]
        public TargetMinMaxValues CategoryValue { get; set; }
    }
}
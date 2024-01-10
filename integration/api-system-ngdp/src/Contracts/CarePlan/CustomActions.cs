﻿// <copyright file="CustomActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Ngdp.Contracts.CarePlan
{
    public class CustomActions
    {
        [Description("Action Name")]
        public string ActionName { get; set; }
        [Description("Description")]
        public string Description { get; set; }
    }
}

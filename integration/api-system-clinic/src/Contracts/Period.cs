// <copyright file="Period.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Time range defined by start and end date/time")]
    public class Period
    {
        [Description("Starting time with inclusive boundary")]
        public DateTimeOffset? Start { get; set; }

        [Description("End time with inclusive boundary, if not ongoing")]
        public DateTimeOffset? End { get; set; }
    }
}
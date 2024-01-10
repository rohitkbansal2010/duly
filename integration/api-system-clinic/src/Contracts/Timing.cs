// <copyright file="Timing.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("A timing schedule that specifies an event that may occur multiple times")]
    public class Timing
    {
        [Description("Element indicating an event that occurs multiple times")]
        public Repeat Repeat { get; set; }
    }
}
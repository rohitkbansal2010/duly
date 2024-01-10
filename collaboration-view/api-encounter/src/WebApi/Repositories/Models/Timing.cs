// <copyright file="Timing.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// A timing schedule that specifies an event that may occur multiple times.
    /// </summary>
    internal class Timing
    {
        /// <summary>
        /// Element indicating an event that occurs multiple times.
        /// </summary>
        public Repeat Repeat { get; set; }
    }
}
// <copyright file="Period.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Time range defined by start and end date/time.
    /// </summary>
    internal class Period
    {
        /// <summary>
        /// Starting time with inclusive boundary.
        /// </summary>
        public DateTimeOffset? Start { get; set; }

        /// <summary>
        /// End time with inclusive boundary, if not ongoing.
        /// </summary>
        public DateTimeOffset? End { get; set; }
    }
}
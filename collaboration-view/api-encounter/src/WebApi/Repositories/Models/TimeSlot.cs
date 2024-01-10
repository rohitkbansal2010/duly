// -----------------------------------------------------------------------
// <copyright file="TimeSlot.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Represents the time slot with start and end period.
    /// </summary>
    internal class TimeSlot
    {
        /// <summary>
        /// When action is to take place.
        /// </summary>
        [Required]
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        /// When action is to conclude.
        /// </summary>
        public DateTimeOffset EndTime { get; set; }

        public int GetDurationInMinutes()
        {
            return (int)Math.Round((EndTime - StartTime).TotalMinutes, 0, MidpointRounding.AwayFromZero);
        }
    }
}

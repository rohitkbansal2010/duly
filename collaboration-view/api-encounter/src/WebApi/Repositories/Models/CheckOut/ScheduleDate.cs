// <copyright file="ScheduleDate.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut
{
    public class ScheduleDate
    {
        /// <summary>
        /// Date of proposed appointments' time slots.
        /// </summary>
        public DateTimeOffset Date { get; init; }

        /// <summary>
        /// Proposed appointments' time slots.
        /// </summary>
        public Slot[] Slots { get; init; }
    }
}
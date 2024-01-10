// <copyright file="Slot.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut
{
    public class Slot
    {
        /// <summary>
        /// Actual time of appointment.
        /// </summary>
        public TimeSpan Time { get; init; }

        /// <summary>
        /// Time which is displayed for patient.
        /// </summary>
        public TimeSpan DisplayTime { get; init; }

        /// <summary>
        /// Arrival time of appointment.
        /// </summary>
        public TimeSpan ArrivalTime { get; init; }
    }
}
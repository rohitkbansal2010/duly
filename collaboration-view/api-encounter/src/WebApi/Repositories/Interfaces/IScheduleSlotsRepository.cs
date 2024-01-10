// <copyright file="IScheduleSlotsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    internal interface IScheduleSlotsRepository
    {
        /// <summary>
        /// Schedule appointment based on <see cref="model"/>.
        /// </summary>
        /// <param name="model">Description of the required appointment to be created.</param>
        /// <returns>Scheduled appointment information.</returns>
        Task<ScheduledAppointment> ScheduleAppointmentForPatient(ScheduleAppointmentModel model);
    }
}

// <copyright file="IGetSlotDataRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.CheckOut.CheckOutDetails"/> model.
    /// </summary>
    internal interface IGetSlotDataRepository
    {
        /// <summary>
        /// Get providers.
        /// </summary>
        /// <param name="appointmentId">Appointment Id.</param>
        /// <returns>Returns <see cref="AppointmentByCsnId"/>.</returns>
        public Task<AppointmentByCsnId> GetProvidersByAppointmentIdAsync(string appointmentId);
    }
}

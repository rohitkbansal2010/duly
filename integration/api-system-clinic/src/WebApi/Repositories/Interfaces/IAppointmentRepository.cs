// <copyright file="IAppointmentRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for scheduling appointments.
    /// </summary>
    public interface IAppointmentRepository
    {
        /// <summary>
        /// Schedule an appointment and return created instance of <see cref="ScheduledAppointment"/>.
        /// </summary>
        /// <param name="model">Request object with parameters.</param>
        /// <returns>Instance of <see cref="ScheduledAppointment"/>.</returns>
        Task<ScheduledAppointment> ScheduleAppointmentAsync(ScheduleAppointmentModel model);
    }
}

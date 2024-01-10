// <copyright file="IAppointmentRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Appointment"/> entities.
    /// </summary>
    public interface IAppointmentRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Appointment"/> which match with the filter.
        /// </summary>
        /// <param name="departmentId">Identifier of the department.</param>
        /// <param name="startDate">Filter by start date.</param>
        /// <param name="endDate">Filter by end date.</param>
        /// <param name="includedVisitTypes">Visit type Ids that should be included.</param>
        /// <param name="excludedAppointmentStatuses">Appointment statuses that should be excluded, if null or empty includes all.</param>
        /// <returns>Filtered items of <see cref="Appointment"/>.</returns>
        Task<IEnumerable<Appointment>> GetAppointmentsForLocationByDateRangeAsync(
            string departmentId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string[] includedVisitTypes,
            AppointmentStatusParam[] excludedAppointmentStatuses = null);

        /// <summary>
        /// Retrieve an <see cref="Appointment"/> which match with the CSN Id.
        /// </summary>
        /// <param name="csnId">Appointment CSN Id.</param>
        /// <returns>An item of <see cref="Appointment"/>.</returns>
        Task<Appointment> GetAppointmentByCsnId(string csnId);

        /// <summary>
        /// Retrieve all items of <see cref="Appointment"/> for the patient which has an appointment with specific CSN Id.
        /// </summary>
        /// <param name="csnId">Appointment CSN Id.</param>
        /// <param name="startDate">Filter by start date.</param>
        /// <param name="endDate">Filter by end date.</param>
        /// <param name="includedAppointmentStatuses">Appointment statuses that should be included.</param>
        /// <returns>Filtered items of <see cref="Appointment"/>.</returns>
        Task<IEnumerable<Appointment>> GetAppointmentsForPatientByCsnIdAsync(
            string csnId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            AppointmentStatusParam[] includedAppointmentStatuses);

        /// <summary>
        /// Retrieve all items of <see cref="ReferralAppointment"/> which match with the referral Id.
        /// </summary>
        /// <param name="referralId">Referral Id.</param>
        /// <returns>Items of <see cref="ReferralAppointment"/> matches with referral id.</returns>
        Task<IEnumerable<ReferralAppointment>> GetReferralAppointmentsByReferralIdAsync(string referralId);
    }
}
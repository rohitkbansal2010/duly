// <copyright file="IAppointmentAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// An adapter for working with "Appointments" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IAppointmentAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="Appointment"/> by <see cref="searchCriteria"/>.
        /// </summary>
        /// <param name="searchCriteria">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref="Appointment"/>.</returns>
        Task<IEnumerable<Appointment>> FindAppointmentsAsync(AppointmentSearchCriteria searchCriteria);

        /// <summary>
        /// Finds matching <see cref="Appointment"/> by CSN Id/>.
        /// </summary>
        /// <param name="csnId">Appointment CSN Id.</param>
        /// <returns>Item of <see cref="Appointment"/>.</returns>
        Task<Appointment> FindAppointmentByCsnIdAsync(string csnId);

        /// <summary>
        /// Finds matching items of <see cref="Appointment"/> for the patient which has the appointment
        /// with specific CSN Id and other criteria from specific <see cref="searchCriteria"/>.
        /// </summary>
        /// <param name="searchCriteria">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref="Appointment"/>.</returns>
        Task<IEnumerable<Appointment>> FindAppointmentsForPatientByCsnIdAsync(AppointmentSearchCriteria searchCriteria);

        /// <summary>
        /// Finds matching items of <see cref="ReferralAppointment"/> by referral Id/>.
        /// </summary>
        /// <param name="referralId">Referral Id.</param>
        /// <returns>Item of <see cref="ReferralAppointment"/>.</returns>
        Task<IEnumerable<ReferralAppointment>> FindReferralAppointmentsByReferralIdAsync(string referralId);
    }
}
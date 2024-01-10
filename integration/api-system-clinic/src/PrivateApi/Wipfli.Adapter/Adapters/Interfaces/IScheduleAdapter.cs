// <copyright file="IScheduleAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Wipfli.Adapter.Client;

namespace Wipfli.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// Provides schedule information.
    /// </summary>
    public interface IScheduleAdapter
    {
        /// <summary>
        /// Returns availability for provider with given location.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Schedule information for provided search criteria.</returns>
        Task<Schedule> GetScheduleForProvider(ScheduleDaysForProviderSearchCriteria searchCriteria);

        /// <summary>
        /// Schedule appointment for the patient.
        /// </summary>
        /// <param name="request">Schedule request.</param>
        /// <returns>Scheduled appointment with insurance.</returns>
        Task<ScheduledAppointmentWithInsurance> ScheduleAppointment(ScheduleAppointmentWithInsuranceRequest request);
    }
}
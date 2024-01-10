// <copyright file="ScheduleAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Wipfli.Adapter.Adapters.Interfaces;
using Wipfli.Adapter.Client;

namespace Wipfli.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IScheduleAdapter"/>
    /// </summary>
    internal class ScheduleAdapter : IScheduleAdapter
    {
        private readonly IWipfliClient _client;

        public ScheduleAdapter(IWipfliClient client)
        {
            _client = client;
        }

        public Task<Schedule> GetScheduleForProvider(ScheduleDaysForProviderSearchCriteria searchCriteria)
        {
            return _client.GetScheduleDaysForProvider(searchCriteria);
        }

        public Task<ScheduledAppointmentWithInsurance> ScheduleAppointment(ScheduleAppointmentWithInsuranceRequest request)
        {
            return _client.ScheduleAppointmentWithInsurance(request);
        }
    }
}
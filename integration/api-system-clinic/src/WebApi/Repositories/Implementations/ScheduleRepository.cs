// <copyright file="ScheduleRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Api.Repositories.Mappings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wipfli.Adapter.Adapters.Interfaces;
using Wipfli.Adapter.Client;
using ScheduleDay = Duly.Clinic.Contracts.ScheduleDay;

namespace Duly.Clinic.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IScheduleRepository"/>
    /// </summary>
    internal class ScheduleRepository : IScheduleRepository
    {
        private readonly IScheduleAdapter _scheduleAdapter;
        private readonly IMapper _mapper;

        public ScheduleRepository(
            IScheduleAdapter scheduleAdapter,
            IMapper mapper)
        {
            _scheduleAdapter = scheduleAdapter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ScheduleDay>> GetScheduleAsync(DateTime startDate, DateTime endDate, string providerId, string departmentId, string visitTypeId)
        {
            var splittedProviderId = providerId.SplitIdentifier();

            var searchCriteria = new ScheduleDaysForProviderSearchCriteria
            {
                StartDate = startDate,
                EndDate = endDate,
                ProviderID = splittedProviderId.ID,
                ProviderIDType = splittedProviderId.Type,
                DepartmentIDs = new[] { departmentId.SplitIdentifier() },
                VisitTypeIDs = new[] { visitTypeId.SplitIdentifier() }
            };
            var schedule = await _scheduleAdapter.GetScheduleForProvider(searchCriteria);

            var systemSchedule = _mapper.Map<IEnumerable<ScheduleDay>>(schedule.ScheduleDays);
            return systemSchedule;
        }
    }
}

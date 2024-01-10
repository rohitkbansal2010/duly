// -----------------------------------------------------------------------
// <copyright file="GetSlotsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IGetSlotsRepository"/>
    /// </summary>
    internal class GetSlotsRepository : IGetSlotsRepository
    {
        private readonly IEncounterContext _encounterContext;
        private readonly IClient _client;
        private readonly IMapper _mapper;

        public GetSlotsRepository(
            IEncounterContext encounterContext,
            IMapper mapper,
            IClient client)
        {
            _encounterContext = encounterContext;
            _mapper = mapper;
            _client = client;
        }

        public async Task<Models.CheckOut.ScheduleDate[]> GetOpenScheduleDatesAsync(string providerId, string departmentId, string visitTypeId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var systemScheduleDays = await _client.SlotsAsync(
                startDate,
                endDate,
                providerId,
                departmentId,
                visitTypeId,
                _encounterContext.GetXCorrelationId());

            return _mapper.Map<Models.CheckOut.ScheduleDate[]>(systemScheduleDays);
        }
    }
}

// <copyright file="ScheduleFollowupService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IScheduleFollowupService"/>
    /// </summary>
    public class ScheduleFollowupService : IScheduleFollowupService
    {
        private readonly IMapper _mapper;
        private readonly IScheduleFollowupRepository _repository;

        public ScheduleFollowupService(IMapper mapper, IScheduleFollowupRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<int> DataPostedToEpicAsync(int id)
        {
            var DataPostedToEpic = await _repository.PostDataPostedToEpic(id);
            return DataPostedToEpic;
        }

        public async Task<int> PostScheduleFollowup(Contracts.ScheduleFollowUp request)
        {
            if (string.IsNullOrEmpty(request.Type))
                request.Type = "Schedule";

            var requestScheculeFollowup = _mapper.Map<Models.CheckOut.ScheduleFollowUp>(request);
            var scheduleFollowupResponse = await _repository.PostScheduleFollowup(requestScheculeFollowup);

            return scheduleFollowupResponse;
        }
    }
}
// <copyright file="ReferralService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IReferralService"/>
    /// </summary>
    public class ReferralService : IReferralService
    {
        private readonly IMapper _mapper;
        private readonly IReferralRepository _repository;

        public ReferralService(IMapper mapper, IReferralRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<int> DataPostedToEpicAsync(int id)
        {
            var DataPostedToEpic = await _repository.PostDataPostedToEpic(id);
            return DataPostedToEpic;
        }

        public async Task<int> PostReferral(Contracts.ScheduleReferral request)
        {
            if (string.IsNullOrEmpty(request.Type))
                request.Type = "Referral";

            var requestReferral = _mapper.Map<Models.CheckOut.ScheduleReferral>(request);
            var referralResponse = await _repository.PostReferral(requestReferral);

            return referralResponse;
        }
    }
}
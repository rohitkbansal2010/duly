// <copyright file="CheckOutDetailsService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Extensions;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ICheckOutDetailsService"/>
    /// </summary>
    internal class CheckOutDetailsService : ICheckOutDetailsservice
    {
        private readonly IMapper _mapper;
        private readonly ICheckOutDetailsRepository _repository;

        public CheckOutDetailsService(
            IMapper mapper,
            ICheckOutDetailsRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CheckOutDetails> GetCheckOutDetailsAsync(string appointmentId)
        {
            var checkOutDetails = await _repository.GetCheckOutDetailsByIdAsync(appointmentId);
            var checkOutDetails1 = new CheckOutDetails();
            checkOutDetails1.Message = checkOutDetails.Message;
            checkOutDetails1.LabDetailsList = _mapper.Map<List<GetLabOrImaging>>(checkOutDetails.LabDetailsList);
            checkOutDetails1.ScheduleFollowUpList = _mapper.Map<List<ScheduleReferral>>(checkOutDetails.ScheduleFollowUpList);
            return checkOutDetails1;
        }
    }
}

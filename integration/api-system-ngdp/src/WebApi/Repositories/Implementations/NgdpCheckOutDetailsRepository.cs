// <copyright file="NgdpCheckOutDetailsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ICheckOutDetailsRepository"/>
    /// </summary>
    internal class NgdpCheckOutDetailsRepository : ICheckOutDetailsRepository
    {
        private readonly ICheckOutDetailsAdapter _adapter;
        private readonly IMapper _mapper;

        public NgdpCheckOutDetailsRepository(ICheckOutDetailsAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        public async Task<CheckOutDetails> GetCheckOutDetailsAsync(string appointmentId)
        {
            var ngdpCheckOutDetails = await _adapter.FindCheckOutDetailsAsync(appointmentId);
            var checkOutDetails = new CheckOutDetails();
            checkOutDetails.Message = ngdpCheckOutDetails.Message;
            checkOutDetails.LabDetailsList = _mapper.Map<List<GetLabOrImagingDetails>>(ngdpCheckOutDetails.LabDetailsList);
            checkOutDetails.ScheduleFollowUpList = _mapper.Map<List<ReferralDetail>>(ngdpCheckOutDetails.ScheduleFollowUpList);
            return checkOutDetails;
        }
    }
}
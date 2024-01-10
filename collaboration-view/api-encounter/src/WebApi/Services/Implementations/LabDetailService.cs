// <copyright file="LabDetailService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ILabDetailService"/>
    /// </summary>
    internal class LabDetailService : ILabDetailService
    {
        private readonly IMapper _mapper;
        private readonly ILabDetailRepository _repository;

        public LabDetailService(IMapper mapper, ILabDetailRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<int> PostLabDetailAsync(LabDetail request)
        {
            if (string.IsNullOrEmpty(request.Type))
                request.Type = "Lab";

            var requestLabDetails = _mapper.Map<Models.LabDetails>(request);
            var responseLabDetails = await _repository.PostLabDetailAsync(requestLabDetails);

            return responseLabDetails;
        }

        public async Task<IEnumerable<LabLocation>> GetLabLocationByLatLngAsync(string lat, string lng)
        {
            var ngdpLabs = (await _repository.GetLabLocationByLatLngAsync(lat, lng)).ToArray();
            var labsLocation = ngdpLabs.Select(appointment => new LabLocation()
            {
               LabId = appointment.LabId,
               LabName = appointment.LabName,
               ExternalLabYn = appointment.ExternalLabYn,
               LabLlbId = appointment.LabLlbId,
               LlbName = appointment.LlbName,
               LlbAddressLn1 = appointment.LlbAddressLn1,
               LlbAddressLn2 = appointment.LlbAddressLn2,
               LLCity = appointment.LLCity,
               LLState = appointment.LLState,
               LLZip = appointment.LLZip,
               LabLatitude = appointment.LabLatitude,
               LabLongitude = appointment.LabLongitude,
               Distance = appointment.Distance,
               ContactNumber = appointment.ContactNumber,
               WorkingHours = appointment.WorkingHours
            }).ToArray();
            return labsLocation;
        }
    }
}
// <copyright file="ProviderService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IProviderService"/>
    /// </summary>
    public class ProviderService : IProviderService
    {
        private readonly IMapper _mapper;
        private readonly IProviderRepository _repository;
        public ProviderService(IMapper mapper, IProviderRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<Provider>> GetProvidersByLatLngAsync(string lat, string lng, string providerType)
        {
            var ngdpProviders = (await _repository.GetProvidersByLatLngAsync(lat, lng, providerType)).ToArray();
            var providers = ngdpProviders.Select(appointment => new Provider()
            {
                ID = appointment.ID,
                LocationId = appointment.Location_Id,
                ProviderId = appointment.Provider_Id,
                ProviderDisplayName = appointment.ProviderDisplayName,
                ProviderName = appointment.Provider_Name,
                ProviderPhotoURL = appointment.Provider_Photo_URL,
                Distance = appointment.Distance,
                LocationLatitudeCoordinate = appointment.Location_Latitude_coordinate,
                LocationLongitudeCoordinate = appointment.Location_Longitude_coordinate,
                City = appointment.City,
                ProviderSpecialty = appointment.Provider_Specialty,
                LocationName = appointment.Location_Name,
                LocationAdd_1 = appointment.Location_Add_1,
                LocationState = appointment.Location_State,
                LocationZip = appointment.Location_Zip,
                Department_Id = appointment.Department_Id
            }).ToArray();
            return providers;
        }

        public async Task<IEnumerable<ProviderDetails>> GetProviderDetailsAsync(string providerIds)
        {
            var providerDetails = await _repository.GetProviderDetailsAsync(providerIds);
            var result = _mapper.Map<IEnumerable<ProviderDetails>>(providerDetails);
            return result;
        }
    }
}

// <copyright file="ProviderLocationConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps <see cref="AdapterModels.ProviderLocation"/> into <see cref="ProviderLocation"/>.
    /// </summary>
    public class ProviderLocationConverter : ITypeConverter<AdapterModels.ProviderLocation, ProviderLocation>
    {
        public ProviderLocation Convert(AdapterModels.ProviderLocation source, ProviderLocation destination, ResolutionContext context)
        {
            return new ProviderLocation
            {
                Id = source.Id,
                Provider_Id = source.Provider_Id,
                Provider_Name = source.Provider_Name,
                ProviderDisplayName = source.ProviderDisplayName,
                Provider_Photo_URL = source.Provider_Photo_URL,
                Location_Id = source.Location_Id,
                Location_Latitude_coordinate = source.Location_Latitude_coordinate,
                Location_Longitude_coordinate = source.Location_Longitude_coordinate,
                City = source.City,
                Provider_Specialty = source.Provider_Specialty,
                Distance = source.Distance,
                Location_Name = source.Location_Name,
                Location_Add_1 = source.Location_Add_1,
                Location_State = source.Location_State,
                Location_Zip = source.Location_Zip,
                Department_Id = source.Department_ID
            };
    }
}
}
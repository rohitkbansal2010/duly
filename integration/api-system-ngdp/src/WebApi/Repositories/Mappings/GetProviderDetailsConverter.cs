// <copyright file="GetProviderDetailsConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps <see cref="AdapterModels.LabDetails"/> into <see cref="LabDetails"/>.
    /// </summary>
    public class GetProviderDetailsConverter : ITypeConverter<AdapterModels.ProviderDetails, ProviderDetails>
    {
        ProviderDetails ITypeConverter<AdapterModels.ProviderDetails, ProviderDetails>.Convert(AdapterModels.ProviderDetails source, ProviderDetails destination, ResolutionContext context)
        {
            return new ProviderDetails
            {
                ID = source.ID,
                LocationId = source.LocationId,
                ProviderId = source.ProviderId,
                ProviderName = source.ProviderName,
                ProviderDisplayName = source.ProviderDisplayName,
                ProviderPhotoURL = source.ProviderPhotoURL,
                LocationLatitudeCoordinate = source.LocationLatitudeCoordinate,
                LocationLongitudeCoordinate = source.LocationLongitudeCoordinate,
                City = source.City,
                ProviderSpecialty = source.ProviderSpecialty,
                Distance = source.Distance,
                LocationName = source.LocationName,
                LocationAdd_1 = source.LocationAdd_1,
                LocationState = source.LocationState,
                LocationZip = source.LocationZip
            };
        }
    }
}
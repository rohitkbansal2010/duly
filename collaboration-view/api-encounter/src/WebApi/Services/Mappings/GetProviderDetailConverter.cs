// <copyright file="GetProviderDetailConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    public class GetProviderDetailConverter : ITypeConverter<Models.ProviderDetails, Contracts.ProviderDetails>
    {
        public Contracts.ProviderDetails Convert(Models.ProviderDetails source, Contracts.ProviderDetails destination, ResolutionContext context)
        {
            return new Contracts.ProviderDetails
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

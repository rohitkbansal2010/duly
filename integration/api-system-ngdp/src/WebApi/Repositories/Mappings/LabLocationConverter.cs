// <copyright file="LabLocationConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps <see cref="AdapterModels.LabLocation"/> into <see cref="LabLocation"/>.
    /// </summary>
    public class LabLocationConverter : ITypeConverter<AdapterModels.LabLocation, LabLocation>
    {
        public LabLocation Convert(AdapterModels.LabLocation source, LabLocation destination, ResolutionContext context)
        {
            return new LabLocation
            {
                LabId = source.LabId,
                LabName = source.LabName,
                ExternalLabYn = source.ExternalLabYn,
                LabLlbId = source.LabLlbId,
                LlbName = source.LlbName,
                LlbAddressLn1 = source.LlbAddressLn1,
                LlbAddressLn2 = source.LlbAddressLn2,
                LLCity = source.LLCity,
                LLState = source.LLState,
                LLZip = source.LLZip,
                LabLatitude = source.LabLatitude,
                LabLongitude = source.LabLongitude,
                Distance = source.Distance,
                ContactNumber = source.ContactNumber,
                WorkingHours = source.WorkingHours
            };
    }
}
}
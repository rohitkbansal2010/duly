// <copyright file="PharmacyDetailConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps <see cref="Pharmacy"/> into <see cref="AdapterModels.Pharmacy"/>.
    /// </summary>
    public class GetPharmacyDetailConverter : ITypeConverter<AdapterModels.Pharmacy, Pharmacy>
    {
        public Pharmacy Convert(AdapterModels.Pharmacy source, Pharmacy destination, ResolutionContext context)
        {
            return new Pharmacy
            {
                PharmacyName = source.PharmacyName,
                PharmacyID = source.PharmacyID,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                State = source.State,
                PhoneNumber = source.PhoneNumber,
                ClosingTime = source.ClosingTime,
                ZipCode = source.ZipCode,
                City = source.City
            };
        }
    }
}
// <copyright file="PharmacyConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using System;
using System.Linq;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    internal class PharmacyConverter : ITypeConverter<Models.Pharmacy, Pharmacy>
    {
        public Pharmacy Convert(Models.Pharmacy source, Pharmacy destination, ResolutionContext context)
        {
            return new Pharmacy
            {
                PharmacyName = source.PharmacyName,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                State = source.State,
                PhoneNumber = source.PhoneNumber,
                ClosingTime = source.ClosingTime,
                ZipCode = source.ZipCode,
                PharmacyID = source.PharmacyID,
                City = source.City
            };
        }
    }
}

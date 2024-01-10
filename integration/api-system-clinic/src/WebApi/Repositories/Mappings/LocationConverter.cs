// <copyright file="LocationConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using AutoMapper;
using Duly.Clinic.Contracts;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    public class LocationConverter : ITypeConverter<R4.Encounter.LocationComponent, Location>
    {
        public Location Convert(R4.Encounter.LocationComponent source, Location destination, ResolutionContext context)
        {
            return new Location
            {
                Title = source.Location.Display ?? source.Location.Url.ToString()
            };
        }
    }
}
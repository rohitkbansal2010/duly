// <copyright file="ImmunizationConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps <see cref="AdapterModels.Immunization"/> into <see cref="Immunization"/>.
    /// </summary>
    public class ImmunizationConverter : ITypeConverter<AdapterModels.Immunization, Immunization>
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public ImmunizationConverter(ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public Immunization Convert(AdapterModels.Immunization source, Immunization destination, ResolutionContext context)
        {
            return new Immunization
            {
                Patient = BuildPatient(source),
                VaccineName = source.VaccineName,
                DueDate = _timeZoneConverter.ToCstDateTimeOffset(source.DueDate),
                Status = (DueStatus)source.StatusId
            };
        }

        private static Patient BuildPatient(AdapterModels.Immunization source)
        {
            return new()
            {
                Id = source.PatientExternalId
            };
        }
    }
}
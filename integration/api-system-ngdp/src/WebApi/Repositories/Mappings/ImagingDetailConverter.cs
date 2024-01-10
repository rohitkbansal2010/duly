// <copyright file="ImagingDetailConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    public class ImagingDetailConverter : ITypeConverter<ImagingDetail, AdapterModels.ImagingDetail>
    {
        public AdapterModels.ImagingDetail Convert(ImagingDetail source, AdapterModels.ImagingDetail destination, ResolutionContext context)
        {
            return new AdapterModels.ImagingDetail
            {
                ID = source.ID,
                Type = source.Type,
                ImagingType = source.ImagingType,
                Appointment_ID = source.Appointment_ID,
                Patient_ID = source.Patient_ID,
                Provider_ID = source.Provider_ID,
                Location_ID = source.Location_ID,
                BookingSlot = source.BookingSlot,
                AptScheduleDate = source.AptScheduleDate,
                ImagingLocation = source.ImagingLocation,
                Skipped = source.Skipped
            };
        }
    }
}
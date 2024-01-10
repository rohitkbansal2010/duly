// <copyright file="GetLabDetailsConverter.cs" company="Duly Health and Care">
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
    public class GetLabDetailsConverter : ITypeConverter<AdapterModels.GetLabOrImagingDetails, GetLabOrImagingDetails>
    {
        GetLabOrImagingDetails ITypeConverter<AdapterModels.GetLabOrImagingDetails, GetLabOrImagingDetails>.Convert(AdapterModels.GetLabOrImagingDetails source, GetLabOrImagingDetails destination, ResolutionContext context)
        {
            return new GetLabOrImagingDetails
            {
                ID = source.ID,
                Type = source.Type,
                Lab_ID = source.Lab_ID,
                Lab_Location = source.Lab_Location,
                Lab_Name = source.Lab_Name,
                CreatedDate = source.CreatedDate,
                Appointment_ID = source.Appointment_ID,
                Patient_ID = source.Patient_ID,
                Provider_ID = source.Provider_ID,
                Location_ID = source.Location_ID,
                BookingSlot = source.BookingSlot,
                AptScheduleDate = source.AptScheduleDate,
                ImagingLocation = source.ImagingLocation,
                ImagingType = source.ImagingType,
                Skipped = source.Skipped
            };
        }
    }
}
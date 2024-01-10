// <copyright file="ImagingDetailConverter.cs" company="Duly Health and Care">
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
    public class ImagingDetailConverter : ITypeConverter<Contracts.ImagingDetail, Models.ImagingDetails>
    {
        public ImagingDetails Convert(ImagingDetail source, ImagingDetails destination, ResolutionContext context)
        {
            return new Models.ImagingDetails
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
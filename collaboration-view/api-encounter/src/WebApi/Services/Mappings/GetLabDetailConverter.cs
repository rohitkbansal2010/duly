// <copyright file="GetLabDetailConverter.cs" company="Duly Health and Care">
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
    public class GetLabDetailConverter : ITypeConverter<Models.GetLabOrImaging, Contracts.GetLabOrImaging>
    {
        public Contracts.GetLabOrImaging Convert(Models.GetLabOrImaging source, Contracts.GetLabOrImaging destination, ResolutionContext context)
        {
            return new Contracts.GetLabOrImaging
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

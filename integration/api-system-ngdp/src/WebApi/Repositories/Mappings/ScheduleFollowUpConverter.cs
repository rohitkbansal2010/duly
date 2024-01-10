// <copyright file="ScheduleFollowUpConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    public class ScheduleFollowUpConverter : ITypeConverter<Contracts.ScheduleFollowUp, AdapterModels.ScheduleFollowUp>
    {
        public AdapterModels.ScheduleFollowUp Convert(Contracts.ScheduleFollowUp source, AdapterModels.ScheduleFollowUp destination, ResolutionContext context)
        {
            return new AdapterModels.ScheduleFollowUp
            {
                ID = source.ID,
                Provider_ID = source.Provider_ID,
                Patient_ID = source.Patient_ID,
                AptType = source.AptType,
                AptFormat = source.AptFormat,
                Location_ID = source.Location_ID,
                AptScheduleDate = source.AptScheduleDate,
                BookingSlot = source.BookingSlot,
                RefVisitType = source.RefVisitType,
                Created_Date = source.Created_Date,
                Type = source.Type,
                Appointment_Id = source.Appointment_Id,
                Skipped = source.Skipped
            };
        }
    }
}
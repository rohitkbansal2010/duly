﻿// <copyright file="ReferralConverter.cs" company="Duly Health and Care">
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
    public class ReferralConverter : ITypeConverter<Contracts.ScheduleFollowUp, Models.ScheduleFollowUp>
    {
        public Models.ScheduleFollowUp Convert(Contracts.ScheduleFollowUp source, Models.ScheduleFollowUp destination, ResolutionContext context)
        {
            return new Models.ScheduleFollowUp
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
                Skipped = source.Skipped,
                VisitTypeId = source.VisitTypeId
            };
        }
    }
}

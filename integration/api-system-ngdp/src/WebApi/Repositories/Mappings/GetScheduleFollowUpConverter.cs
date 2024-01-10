// <copyright file="GetScheduleFollowUpConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Models;
using Duly.Ngdp.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Repositories.Mappings
{
    public class GetScheduleFollowUpConverter : ITypeConverter<AdapterModels.ReferralDetail, Contracts.ReferralDetail>
    {
        public Contracts.ReferralDetail Convert(AdapterModels.ReferralDetail source, Contracts.ReferralDetail destination, ResolutionContext context)
        {
            return new Contracts.ReferralDetail
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
                Department_Id = source.Department_Id
            };
        }
    }
}

// <copyright file="ScheduleSlotsConverter.cs" company="Duly Health and Care">
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
    public class ScheduleSlotsConverter : ITypeConverter<Models.ScheduledAppointment, Contracts.ScheduleAppointmentResult>
    {
        public Contracts.ScheduleAppointmentResult Convert(Models.ScheduledAppointment source, Contracts.ScheduleAppointmentResult destination, ResolutionContext context)
        {
            return new Contracts.ScheduleAppointmentResult
            {
                Id = source.Id,
                DateTime = source.DateTime,
                DurationInMinutes = source.DurationInMinutes,
                TimeZone = source.TimeZone,
                ScheduledTime = source.ScheduledTime
            };
        }
    }
}

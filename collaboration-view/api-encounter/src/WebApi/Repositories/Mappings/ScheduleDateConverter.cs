// <copyright file="ScheduleDateConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Mappings
{
    /// <summary>
    /// Maps <see cref="ScheduleDay"/> into <see cref="Models.ScheduleDate"/>.
    /// </summary>
    public class ScheduleDateConverter :
        ITypeConverter<ScheduleDay, ScheduleDate>,
        ITypeConverter<Clinic.Api.Client.ScheduledAppointment, Models.CheckOut.ScheduledAppointment>
    {
        private const string CsnKey = "CSN|";

        public ScheduleDate Convert(ScheduleDay source, ScheduleDate destination, ResolutionContext context)
        {
            return new ScheduleDate
            {
                Date = source.Date,
                Slots = context.Mapper.Map<Models.CheckOut.Slot[]>(source.Slots)
            };
        }

        public Models.CheckOut.ScheduledAppointment Convert(Clinic.Api.Client.ScheduledAppointment source, Models.CheckOut.ScheduledAppointment destination, ResolutionContext context)
        {
            return new Models.CheckOut.ScheduledAppointment
            {
                Id = BuildId(source.ContactIds),
                DateTime = BuildDateTime(source),
                DurationInMinutes = source.DurationInMinutes.GetValueOrDefault(),
                TimeZone = source.Department.OfficialTimeZone.Title,
                ScheduledTime = source.ScheduledTime.GetValueOrDefault().UtcDateTime
            };
        }

        private static DateTime BuildDateTime(Clinic.Api.Client.ScheduledAppointment source)
        {
            return source.Date.HasValue ? source.Date.Value.Date.Add(source.Time.GetValueOrDefault()) : DateTime.MinValue;
        }

        private static string BuildId(IEnumerable<string> sourceCollection)
        {
            var id = sourceCollection.FirstOrDefault(s => s.StartsWith(CsnKey));
            return id?[CsnKey.Length..];
        }
    }
}

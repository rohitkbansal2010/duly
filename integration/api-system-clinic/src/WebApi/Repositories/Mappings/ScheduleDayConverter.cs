// <copyright file="ScheduleDayConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Duly.Clinic.Api.Repositories.Mappings
{
    public class ScheduleDayConverter : ITypeConverter<Wipfli.Adapter.Client.ScheduleDay, ScheduleDay>
    {
        public ScheduleDay Convert(Wipfli.Adapter.Client.ScheduleDay source, ScheduleDay destination, ResolutionContext context)
        {
            return new ScheduleDay
            {
                Date = source.Date,
                Slots = context.Mapper.Map<IEnumerable<Slot>>(source.Slots).ToArray()
            };
        }
    }
}

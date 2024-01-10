// <copyright file="ScheduleDateConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract = Duly.CollaborationView.Encounter.Api.Contracts;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;

namespace Duly.CollaborationView.Encounter.Api.Services.Mappings
{
    /// <summary>
    /// Maps <see cref="Repositories.Models.ScheduleDate"/> into <see cref="Contracts.ScheduleDate"/>.
    /// </summary>
    public class ScheduleDateConverter : ITypeConverter<Models.ScheduleDate, Contract.ScheduleDate>
    {
        public Contract.ScheduleDate Convert(Models.ScheduleDate source, Contract.ScheduleDate destination, ResolutionContext context)
        {
            return new Contract.ScheduleDate
            {
                Date = source.Date.DateTime,
                TimeSlots = context.Mapper.Map<IEnumerable<TimeSlots>>(source.Slots)
            };
        }
    }
}

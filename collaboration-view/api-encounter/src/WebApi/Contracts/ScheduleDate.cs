// -----------------------------------------------------------------------
// <copyright file="ScheduleDate.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Duly.Common.Annotations.Json;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Describes time slots availability for the specific date.")]
    public class ScheduleDate
    {
        [Description("Date for which time slots are provided.")]
        [JsonConverter(typeof(ExtendedIsoDateTimeConverter), JsonStringFormatters.JsonIsoDateFormat)]
        [SwaggerSchema(Format = "date")]
        [Required]
        public DateTime Date { get; init; }

        [Description("Time slots for the given date.")]
        [Required]
        public IEnumerable<TimeSlots> TimeSlots { get; init; }
    }
}
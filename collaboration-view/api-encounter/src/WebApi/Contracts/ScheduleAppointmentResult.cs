// -----------------------------------------------------------------------
// <copyright file="ScheduleAppointmentResult.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Common.Annotations.Json;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Model for appointment scheduling Result.")]
    public class ScheduleAppointmentResult
    {
        [Description("Identifier by which this appointment is known")]
        public string Id { get; init; }

        [Description("Scheduled appointment date and time")]
        public DateTime DateTime { get; init; }

        [Description("Duration in minutes")]
        public int DurationInMinutes { get; init; }

        [Description("Time zone.")]
        public string TimeZone { get; init; }

        [Description("When the appointment has been scheduled. date and time")]
        public DateTime ScheduledTime { get; set; }
    }
}
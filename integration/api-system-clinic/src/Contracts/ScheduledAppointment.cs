// <copyright file="ScheduledAppointment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Annotations.Json;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Scheduled appointment for the patient")]
    public class ScheduledAppointment
    {
        [Description("Appointment time")]
        [DataType(DataType.Time)]
        public TimeSpan? Time { get; set; }

        [Description("Appointment date")]
        [JsonConverter(typeof(ExtendedIsoDateTimeConverter), JsonStringFormatters.JsonIsoDateFormat)]
        [SwaggerSchema(Format = "date")]
        public DateTime? Date { get; set; }

        [Description("Duration in minutes")]
        public int? DurationInMinutes { get; set; }

        [Description("Detailed instructions for patient")]
        [Required]
        public string[] PatientInstructions { get; set; }

        [Description("Medical specialist")]
        public ScheduledProvider Provider { get; set; }

        [Description("Appointment physical location")]
        public ScheduledDepartment Department { get; set; }

        [Description("Appointment visit type")]
        public ScheduledVisitType VisitType { get; set; }

        [Description("Appointment patient")]
        public ScheduledPatient Patient { get; set; }

        [Description("Collection of identifiers this appointment is known by")]
        public string[] ContactIds { get; set; }

        [Description("When the appointment has been scheduled. date and time")]
        [JsonConverter(typeof(ExtendedIsoDateTimeConverter), JsonStringFormatters.JsonIsoDateTimeFormat)]
        [SwaggerSchema(Format = "date")]
        public DateTime? ScheduledTime { get; set; }
    }
}

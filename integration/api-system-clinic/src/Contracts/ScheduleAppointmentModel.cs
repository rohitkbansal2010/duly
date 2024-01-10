// <copyright file="ScheduleAppointmentModel.cs" company="Duly Health and Care">
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
    [Description("Represents request for appointment scheduling")]
    public class ScheduleAppointmentModel
    {
        [Description("Date of appointment, Format:yyyy-MM-dd")]
        [Required]
        [JsonConverter(typeof(ExtendedIsoDateTimeConverter), JsonStringFormatters.JsonIsoDateFormat)]
        [SwaggerSchema(Format = "date")]
        public DateTime? Date { get; set; }

        [Description("Time of appointment")]
        [DataType(DataType.Time)]
        [Required]
        public TimeSpan? Time { get; set; }

        [Description("Patient Id")]
        [Required]
        public string PatientId { get; set; }

        [Description("Provider Id")]
        [Required]
        public string ProviderId { get; set; }

        [Description("Department Id")]
        [Required]
        public string DepartmentId { get; set; }

        [Description("Visit type Id")]
        [Required]
        public string VisitTypeId { get; set; }
    }
}

// -----------------------------------------------------------------------
// <copyright file="AppointmentSchedulingModel.cs" company="Duly Health and Care">
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
    [Description("Model for appointment scheduling.")]
    public class AppointmentSchedulingModel
    {
        [Description("Provider Id")]
        [Required]
        public string ProviderId { get; init; }

        [Description("Location Id")]
        [Required]
        public string LocationId { get; init; }

        [Description("Appointment date.")]
        [JsonConverter(typeof(ExtendedIsoDateTimeConverter), JsonStringFormatters.JsonIsoDateFormat)]
        [SwaggerSchema(Format = "date")]
        [Required]
        public DateTimeOffset? Date { get; init; }

        [Description("Appointment time.")]
        [Required]
        public TimeSpan? Time { get; init; }

        [Description("Visit Type Id")]
        public string VisitTypeId { get; set; }
    }
}
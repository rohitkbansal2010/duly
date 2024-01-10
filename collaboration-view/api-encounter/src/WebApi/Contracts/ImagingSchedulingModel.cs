// -----------------------------------------------------------------------
// <copyright file="ImagingSchedulingModel.cs" company="Duly Health and Care">
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
    [Description("Model for appointment scheduling for Imaging")]
    public class ImagingSchedulingModel
    {
        [Description("Provider Id")]
        [Required]
        public List<string> ProviderId { get; init; }

        [Description("Selected Provider Id")]
        public string SelectedProviderId { get; set; }

        [Description("patient Id")]
        public string PatientId { get; set; }

        [Description("Department Id")]
        [Required]
        public string DepartmentId { get; init; }

        [Description("Appointment date.")]
        [JsonConverter(typeof(ExtendedIsoDateTimeConverter), JsonStringFormatters.JsonIsoDateFormat)]
        [SwaggerSchema(Format = "date")]
        [Required]
        public DateTimeOffset Date { get; init; }

        [Description("Appointment time.")]
        [Required]
        public TimeSpan Time { get; init; }
    }
}
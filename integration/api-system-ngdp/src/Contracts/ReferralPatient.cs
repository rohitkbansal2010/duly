// <copyright file="ReferralPatient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Annotations.Json;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Patient for whom referral is created")]
    public class ReferralPatient
    {
        [Description("General info about the patient")]
        [Required]
        public Patient Patient { get; init; }

        [Description("Date of birth")]
        [JsonConverter(typeof(ExtendedIsoDateTimeConverter), JsonStringFormatters.JsonIsoDateFormat)]
        [SwaggerSchema(Format = "date")]
        [Required]
        public DateTime DateOfBirth { get; init; }

        [Description("Human name of the patient")]
        public HumanName Name { get; init; }

        [Description("Address of the patient")]
        public Address Address { get; init; }

        [Description("Contact details (telephone, email, etc.) for a contact")]
        public ContactPoint[] ContactPoints { get; init; }
    }
}
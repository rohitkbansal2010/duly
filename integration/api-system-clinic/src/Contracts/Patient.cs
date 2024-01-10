// <copyright file="Patient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Annotations.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Patient with details")]
    public class Patient
    {
        [Description("Information about an individual receiving health care services")]
        [Required]
        public PatientGeneralInfo PatientGeneralInfo { get; set; }

        [Description("Birth date of the patient")]
        [Required]
        [JsonConverter(typeof(ExtendedIsoDateTimeConverter), JsonStringFormatters.JsonIsoDateFormat)]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Description("Birth gender of the patient")]
        [Required]
        public Gender Gender { get; set; }

        [Description("Identifiers of the patient. Format: (Text|VALUE)")]
        public string[] Identifiers { get; set; }

        [Description("Address of the patient")]
        public List<PatientAddress> Address { get; set; }

        [Description("Phone number of the patient")]
        public List<PhoneNumber> PhoneNumber { get; set; }
    }
}

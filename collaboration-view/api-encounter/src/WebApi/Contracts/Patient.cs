// <copyright file="Patient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Information about an individual receiving health care services")]
    public class Patient
    {
        [Description("General information about the patient")]
        [Required]
        public PatientGeneralInfo GeneralInfo { get; set; }

        [Description("The date of birth for the patient")]
        [Required]
        public DateTime BirthDate { get; set; }

        [Description("Birth gender of the patient")]
        [Required]
        public Gender Gender { get; set; }

        [Description("Image of the patient")]
        public List<Attachment> Photo { get; set; }

        [Description("List of patient address")]
        public List<PatientAddress> PatientAddress { get; set; }

        [Description("List of phone number")]
        public List<PhoneNumber> PhoneNumber { get; set; }
    }
}

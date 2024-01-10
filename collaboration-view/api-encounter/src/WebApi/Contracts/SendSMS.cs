// <copyright file="SendSms.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents parameters using which sms triggers")]
    public class SendSms
    {
        [Required]
        [Description("Appointment Id")]
        public string AppointmentId { get; set; }

        [Required]
        [Description("Patient Id")]
        public string PatientId { get; set; }

        [Required]
        [Description("Pdf Id")]
        public string PdfId { get; set; }

        [Description("Phone no")]
        public string PhoneNumber { get; set; }
    }
}
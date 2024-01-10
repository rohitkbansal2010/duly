// <copyright file="AfterVisitPdf.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Ngdp.Contracts
{
    [Description("Save the after visit pdf data")]

    public class AfterVisitPdf
    {
        [Description("Patient Id")]
        public string PatientId { get; set; }

        [Description("Provider ID")]
        public string ProviderId { get; set; }
        [Description("After Visit Pdf")]
        public string AfterVisitPDF { get; set; }
        [Description("Appointment Id")]
        public long AppointmentId { get; set; }
    }
}

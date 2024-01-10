// -----------------------------------------------------------------------
// <copyright file="AfterVisitPdf.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    internal class AfterVisitPdf
    {
        /// <summary>
        /// Patient Id.
        /// </summary>
        public string PatientId { get; set; }

        /// <summary>
        /// Provider Id.
        /// </summary>
        public string ProviderId { get; set; }

        /// <summary>
        /// After Visit Pdf.
        /// </summary>
        public string AfterVisitPDF { get; set; }

        /// <summary>
        /// Appointment Id.
        /// </summary>
        public string AppointmentId { get; set; }

        /// <summary>
        /// After Visit PhoneNumber.
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
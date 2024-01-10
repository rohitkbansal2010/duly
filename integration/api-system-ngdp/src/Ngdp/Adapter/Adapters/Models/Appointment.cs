// <copyright file="Appointment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    public class Appointment
    {
        /// <summary>
        /// Appointment CSN Id.
        /// </summary>
        public decimal CsnId { get; set; }

        /// <summary>
        /// Appointment Reason.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Appointment length in minutes.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Appointment Status.
        /// Scheduled, Arrived, Canceled...
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// Appointment Start Time.
        /// The time is local time in CST. There is no timezone or Time offset in the timestamp.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Id of a visit type.
        /// </summary>
        public string VisitTypeId { get; set; }

        /// <summary>
        /// A type of the visit.
        /// TREATMENT, BH EST VIDEO VISIT, SIM VISIT...
        /// </summary>
        public string VisitType { get; set; }

        /// <summary>
        /// A display name of the visit.
        /// Established Patient Office Visit.
        /// </summary>
        public string VisitTypeDisplayName { get; set; }

        /// <summary>
        /// Patient External Id.
        /// </summary>
        public string PatientExternalId { get; set; }

        /// <summary>
        /// Provider External Id.
        /// EXTERNAL|28325.
        /// </summary>
        public string ProviderDphoneId { get; set; }

        /// <summary>
        /// Provider NPI Id.
        /// NPI|1124339833.
        /// </summary>
        public string ProviderNpiId { get; set; }

        /// <summary>
        /// Provider name.
        /// FITZGERALD, MICHAEL E.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// String denoting whether this visit is telehealth appointment.
        /// y/n.
        /// </summary>
        public string IsTelehealthVisit { get; set; }

        /// <summary>
        /// Whether this appointment needs Break the Glass flow to retrieve all data.
        /// Possible values - y/n.
        /// </summary>
        public string IsUnderBtg { get; set; }
    }
}
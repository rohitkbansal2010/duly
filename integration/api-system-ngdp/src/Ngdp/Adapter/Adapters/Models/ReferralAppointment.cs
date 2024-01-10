// <copyright file="ReferralAppointment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    /// <summary>
    /// Class represents a content of the table [ReferralAppointment] in the database.
    /// </summary>
    public class ReferralAppointment
    {
        /// <summary>
        /// Referral Id.
        /// </summary>
        public string ReferralId { get; init; }

        /// <summary>
        /// Scheduled Appointment CSN id.
        /// </summary>
        public string AppointmentCSN { get; init; }

        /// <summary>
        /// Scheduled appointment date.
        /// </summary>
        public DateTime? AppointmentDate { get; init; }

        /// <summary>
        /// Scheduled appointment time.
        /// </summary>
        public TimeSpan? AppointmentTime { get; init; }

        /// <summary>
        /// Scheduled appointment time zone.
        /// </summary>
        public string AppointmentTimeZone { get; init; }

        /// <summary>
        /// Scheduled appointment duration in minutes.
        /// </summary>
        public int? AppointmentDurationInMins { get; init; }

        /// <summary>
        /// Provider's display name.
        /// </summary>
        public string ProviderDisplayName { get; init; }

        /// <summary>
        /// Provider's external identity.
        /// </summary>
        public string ProviderExternalId { get; init; }

        /// <summary>
        /// Provider's photo url.
        /// </summary>
        public string ProviderPhotoURL { get; init; }

        /// <summary>
        /// Visit type External Identity.
        /// </summary>
        public string VisitTypeExternalId { get; init; }

        /// <summary>
        /// Department External Identity.
        /// </summary>
        public string DepartmentExternalId { get; init; }

        /// <summary>
        /// Department name.
        /// </summary>
        public string DepartmentName { get; init; }

        /// <summary>
        /// Department street.
        /// </summary>
        public string DepartmentStreet { get; init; }

        /// <summary>
        /// Department city.
        /// </summary>
        public string DepartmentCity { get; init; }

        /// <summary>
        /// Department state.
        /// </summary>
        public string DepartmentState { get; init; }

        /// <summary>
        /// Department zip code.
        /// </summary>
        public string DepartmentZip { get; init; }

        /// <summary>
        /// Date and Time when the referral appointment was scheduled.
        /// </summary>
        public DateTimeOffset? AppointmentScheduledTime { get; init; }
    }
}
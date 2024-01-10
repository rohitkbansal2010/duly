// <copyright file="Appointment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.OmniChannel.Orchestrator.Appointment.Common.Models
{
    /// <summary>
    /// Represents a model of appointment for referral.
    /// </summary>
    public record Appointment
    {
        public string CSN { get; init; }

        public DateTime StartDateTime { get; init; }

        public string TimeZone { get; init; }

        public int DurationInMinutes { get; init; }

        public string ProviderDisplayName { get; init; }

        public string ProviderPhotoUrl { get; init; }

        public string ProviderExternalId { get; init; }

        public string VisitTypeExternalId { get; init; }

        public string DepartmentExternalId { get; init; }

        public string DepartmentName { get; init; }

        public string DepartmentStreetName { get; init; }

        public string DepartmentCity { get; init; }

        public string DepartmentState { get; init; }

        public string DepartmentZipCode { get; init; }

        public string ConfirmationPageUrl { get; init; }

        public string CreationDateTime { get; init; }
    }
}

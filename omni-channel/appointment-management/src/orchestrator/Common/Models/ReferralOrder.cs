// <copyright file="ReferralOrder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.OmniChannel.Orchestrator.Appointment.Common.Models
{
    /// <summary>
    /// Represents a model of referral order record.
    /// </summary>
    public record ReferralOrder
    {
        public string ReferralId { get; init; }

        public string PatientId { get; init; }

        public string PatientPhone { get; init; }

        public string PatientName { get; init; }

        public DateTime PatientDateOfBirth { get; init; }

        public string SpecialtyId { get; init; }

        public string Specialty { get; init; }

        public string ProviderExternalId { get; init; }

        public string ProviderName { get; init; }

        public string ProviderPhotoUrl { get; init; }
    }
}

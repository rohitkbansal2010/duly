// <copyright file="AppointmentOptionsMock.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Configurations;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    internal class AppointmentOptionsMock : AppointmentOptions
    {
        private readonly string[] _convertedIncludedAppointmentVisitTypes;
        private readonly AppointmentStatusParam[] _convertedExcludedAppointmentStatuses;
        private readonly HashSet<string> _convertedNewPatientVisitTypes;
        private readonly AppointmentStatusParam[] _convertedIncludedPatientAppointmentStatuses;
        private readonly HashSet<AppointmentStatus> _convertedRecentPatientAppointmentStatuses;
        private readonly HashSet<AppointmentStatus> _convertedUpcomingPatientAppointmentStatuses;

        public AppointmentOptionsMock(
            string[] convertedIncludedAppointmentVisitTypes,
            AppointmentStatusParam[] convertedExcludedAppointmentStatuses,
            HashSet<string> convertedNewPatientVisitTypes,
            AppointmentStatusParam[] convertedIncludedPatientAppointmentStatuses,
            HashSet<AppointmentStatus> convertedRecentPatientAppointmentStatuses,
            HashSet<AppointmentStatus> convertedUpcomingPatientAppointmentStatuses)
        {
            _convertedIncludedAppointmentVisitTypes = convertedIncludedAppointmentVisitTypes;
            _convertedExcludedAppointmentStatuses = convertedExcludedAppointmentStatuses;
            _convertedNewPatientVisitTypes = convertedNewPatientVisitTypes;
            _convertedIncludedPatientAppointmentStatuses = convertedIncludedPatientAppointmentStatuses;
            _convertedRecentPatientAppointmentStatuses = convertedRecentPatientAppointmentStatuses;
            _convertedUpcomingPatientAppointmentStatuses = convertedUpcomingPatientAppointmentStatuses;
        }

        internal override string[] ConvertedIncludedAppointmentVisitTypes => _convertedIncludedAppointmentVisitTypes;

        internal override AppointmentStatusParam[] ConvertedExcludedAppointmentStatuses => _convertedExcludedAppointmentStatuses;

        internal override HashSet<string> ConvertedNewPatientVisitTypes => _convertedNewPatientVisitTypes;

        internal override AppointmentStatusParam[] ConvertedIncludedPatientAppointmentStatuses =>
            _convertedIncludedPatientAppointmentStatuses;

        internal override HashSet<AppointmentStatus> ConvertedRecentPatientAppointmentStatuses =>
            _convertedRecentPatientAppointmentStatuses;

        internal override HashSet<AppointmentStatus> ConvertedUpcomingPatientAppointmentStatuses =>
            _convertedUpcomingPatientAppointmentStatuses;
    }
}
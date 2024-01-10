// <copyright file="AppointmentOptions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Configurations
{
    /// <summary>
    /// Configuration options for appointment conversion from appSettings.
    /// </summary>
    public class AppointmentOptions
    {
        private string _newPatientVisitTypes;
        private string _excludedAppointmentStatuses;
        private string _includedAppointmentVisitTypes;
        private string _includedPatientAppointmentStatuses;
        private string _recentPatientAppointmentStatuses;
        private string _upcomingPatientAppointmentStatuses;

        /// <summary>
        /// Types of visits patients of which should be treated as new.
        /// </summary>
        public string NewPatientVisitTypes
        {
            get => _newPatientVisitTypes;
            set
            {
                _newPatientVisitTypes = value;
                ConvertedNewPatientVisitTypes = new HashSet<string>(JsonConvert.DeserializeObject<string[]>(_newPatientVisitTypes) ?? Array.Empty<string>());
            }
        }

        /// <summary>
        /// Statuses with which appointments will be excluded.
        /// </summary>
        public string ExcludedAppointmentStatuses
        {
            get => _excludedAppointmentStatuses;
            set
            {
                _excludedAppointmentStatuses = value;
                ConvertedExcludedAppointmentStatuses = JsonConvert.DeserializeObject<AppointmentStatusParam[]>(_excludedAppointmentStatuses);
            }
        }

        /// <summary>
        /// Types of visits with which appointments will be included.
        /// </summary>
        public string IncludedAppointmentVisitTypes
        {
            get => _includedAppointmentVisitTypes;
            set
            {
                _includedAppointmentVisitTypes = value;
                ConvertedIncludedAppointmentVisitTypes = JsonConvert.DeserializeObject<string[]>(_includedAppointmentVisitTypes) ?? Array.Empty<string>();
            }
        }

        /// <summary>
        /// The statuses with which appointments will be included for searching by specific appointment identity.
        /// </summary>
        public string IncludedPatientAppointmentStatuses
        {
            get => _includedPatientAppointmentStatuses;
            set
            {
                _includedPatientAppointmentStatuses = value;
                ConvertedIncludedPatientAppointmentStatuses = JsonConvert.DeserializeObject<AppointmentStatusParam[]>(_includedPatientAppointmentStatuses);
            }
        }

        /// <summary>
        /// The statuses with which appointments will be included into the recent appointments groups.
        /// </summary>
        public string RecentPatientAppointmentStatuses
        {
            get => _recentPatientAppointmentStatuses;
            set
            {
                _recentPatientAppointmentStatuses = value;
                ConvertedRecentPatientAppointmentStatuses = new HashSet<AppointmentStatus>(
                    JsonConvert.DeserializeObject<AppointmentStatus[]>(_recentPatientAppointmentStatuses)
                    ?? Array.Empty<AppointmentStatus>());
            }
        }

        /// <summary>
        /// The statuses with which appointments will be included into the upcoming appointments groups.
        /// </summary>
        public string UpcomingPatientAppointmentStatuses
        {
            get => _upcomingPatientAppointmentStatuses;
            set
            {
                _upcomingPatientAppointmentStatuses = value;
                ConvertedUpcomingPatientAppointmentStatuses = new HashSet<AppointmentStatus>(
                    JsonConvert.DeserializeObject<AppointmentStatus[]>(_upcomingPatientAppointmentStatuses)
                    ?? Array.Empty<AppointmentStatus>());
            }
        }

        /// <summary>
        /// Gets NewPatientVisitTypes as HashSet of string.
        /// </summary>
        internal virtual HashSet<string> ConvertedNewPatientVisitTypes { get; private set; }

        /// <summary>
        /// Gets ExcludedAppointmentStatuses as AppointmentStatusParam[].
        /// </summary>
        internal virtual AppointmentStatusParam[] ConvertedExcludedAppointmentStatuses { get; private set; }

        /// <summary>
        /// Gets IncludedAppointmentVisitTypes as string[].
        /// </summary>
        internal virtual string[] ConvertedIncludedAppointmentVisitTypes { get; private set; }

        /// <summary>
        /// Gets IncludedPatientAppointmentStatuses as AppointmentStatusParam[].
        /// </summary>
        internal virtual AppointmentStatusParam[] ConvertedIncludedPatientAppointmentStatuses { get; private set; }

        /// <summary>
        /// Gets RecentPatientAppointmentStatuses as HashSet of AppointmentStatus.
        /// </summary>
        internal virtual HashSet<AppointmentStatus> ConvertedRecentPatientAppointmentStatuses { get; private set; }

        /// <summary>
        /// Gets UpcomingPatientAppointmentStatuses as HashSet of AppointmentStatus.
        /// </summary>
        internal virtual HashSet<AppointmentStatus> ConvertedUpcomingPatientAppointmentStatuses { get; private set; }
    }
}
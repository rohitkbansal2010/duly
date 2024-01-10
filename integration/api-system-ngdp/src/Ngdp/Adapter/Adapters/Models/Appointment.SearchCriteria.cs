// <copyright file="Appointment.SearchCriteria.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Linq;

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    public class AppointmentSearchCriteria
    {
        /// <summary>
        /// Identity of the department.
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// Identity of the appointment (Contact Serial Number).
        /// </summary>
        public string CsnId { get; set; }

        /// <summary>
        /// Lower bound of the appointments to search in CST timezone.
        /// </summary>
        public DateTime AppointmentTimeLowerBound { get; set; }

        /// <summary>
        /// Upper bound of the appointments to search in CST timezone.
        /// </summary>
        public DateTime AppointmentTimeUpperBound { get; set; }

        /// <summary>
        /// Visit types which should be included by Id.
        /// </summary>
        public string[] IncludedVisitTypeIds { get; set; }

        /// <summary>
        /// Statuses which should be excluded from the result.
        /// </summary>
        public AppointmentStatus[] ExcludedStatuses { get; set; }

        /// <summary>
        /// Statuses which should be included to the result.
        /// </summary>
        public AppointmentStatus[] IncludedStatuses { get; set; }

        public dynamic ConvertToParameters()
        {
            var parameters = new
            {
                DepartmentId,
                ApptTimeLowerBound = AppointmentTimeLowerBound,
                ApptTimeUpperBound = AppointmentTimeUpperBound,
                IncludedVisitTypeIds = BuildIncludedVisitTypeIds(),
                ExcludedStatuses = BuildExcludedStatuses()
            };

            return parameters;
        }

        public dynamic ConvertToParametersForPatient()
        {
            var parameters = new
            {
                CsnId,
                ApptTimeLowerBound = AppointmentTimeLowerBound,
                ApptTimeUpperBound = AppointmentTimeUpperBound,
                IncludedStatuses = BuildIncludedStatuses()
            };

            return parameters;
        }

        private static string AppointmentStatusToString(AppointmentStatus appointmentStatus)
        {
            return appointmentStatus switch
            {
                AppointmentStatus.Arrived
                    or AppointmentStatus.Canceled
                    or AppointmentStatus.Completed
                    or AppointmentStatus.Scheduled
                    or AppointmentStatus.Unresolved => appointmentStatus.ToString(),

                AppointmentStatus.LeftWithoutSeen => "Left without seen",
                AppointmentStatus.NoShow => "No Show",
                AppointmentStatus.ChargeEntered => "Charge Entered",

                _ => throw new ArgumentOutOfRangeException(nameof(appointmentStatus))
            };
        }

        private string BuildExcludedStatuses()
        {
            return ExcludedStatuses == null
                ? string.Empty
                : string.Join(',', ExcludedStatuses.Select(AppointmentStatusToString));
        }

        private string BuildIncludedStatuses()
        {
            return IncludedStatuses == null
                ? string.Empty
                : string.Join(',', IncludedStatuses.Select(AppointmentStatusToString));
        }

        private string BuildIncludedVisitTypeIds()
        {
            return IncludedVisitTypeIds == null
                ? string.Empty
                : string.Join(',', IncludedVisitTypeIds);
        }
    }
}
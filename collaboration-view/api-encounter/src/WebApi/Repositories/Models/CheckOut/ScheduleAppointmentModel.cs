// <copyright file="ScheduleAppointmentModel.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut
{
    public class ScheduleAppointmentModel
    {
        /// <summary>
        /// Appointment's date.
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Appointment's daytime.
        /// </summary>
        public TimeSpan Time { get; set; }

        /// <summary>
        /// Patient's identifier with prefix.
        /// </summary>
        public string PatientId { get; set; }

        /// <summary>
        /// Provider's identifier with prefix.
        /// </summary>
        public string ProviderId { get; set; }

        /// <summary>
        /// Department's identifier with prefix.
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// VisitType's identifier with prefix.
        /// </summary>
        public string VisitTypeId { get; set; }
    }
}

// <copyright file="AppointmentByCsnId.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    public class AppointmentByCsnId
    {
        /// <summary>
        /// Provider department.
        /// </summary>
        public string DepartmentId { get; init; }

        /// <summary>
        /// Type of visit.
        /// </summary>
        public string VisitTypeId { get; init; }

        /// <summary>
        /// Provider Id.
        /// </summary>
        public string ProviderId { get; init; }

        /// <summary>
        /// Patient Id.
        /// </summary>
        public string PatientId { get; init; }
    }
}

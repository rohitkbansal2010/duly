// <copyright file="LabDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    public class DepartmentVisitType
    {
        /// <summary>
        /// Department Id.
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// Visit Type Id.
        /// </summary>
        public string VisitTypeId { get; set; }

        /// <summary>
        /// Provider Id.
        /// </summary>
        public string ProviderId { get; set; }

        /// <summary>
        /// Patient Id.
        /// </summary>
        public string PatientId { get; set; }
    }
}
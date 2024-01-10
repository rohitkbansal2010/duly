// <copyright file="Immunization.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    public class Immunization
    {
        /// <summary>
        /// Patient External Id.
        /// </summary>
        public string PatientExternalId { get; set; }

        /// <summary>
        /// Vaccine name.
        /// </summary>
        public string VaccineName { get; set; }

        /// <summary>
        /// Due date.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Due status id.
        /// </summary>
        public int StatusId { get; set; }
    }
}

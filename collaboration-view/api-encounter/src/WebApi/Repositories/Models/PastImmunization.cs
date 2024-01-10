// <copyright file="PastImmunization.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Record of patient's past immunization, including vaccine and vaccine administration details.
    /// </summary>
    internal class PastImmunization
    {
        /// <summary>
        /// Business identifier for this immunization.
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Past immunization status.
        /// </summary>
        [Required]
        public PastImmunizationStatus Status { get; set; }

        /// <summary>
        /// Administered vaccine.
        /// </summary>
        [Required]
        public PastImmunizationVaccine Vaccine { get; set; }

        /// <summary>
        /// Vaccination administration date and time.
        /// </summary>
        public DateTimeOffset? OccurrenceDateTime { get; set; }

        /// <summary>
        /// Information about the amount of vaccine administered.
        /// </summary>
        public Quantity Dose { get; set; }

        /// <summary>
        /// Vaccination notes.
        /// </summary>
        public string[] Notes { get; set; }

        /// <summary>
        /// Why immunization did not occur.
        /// </summary>
        public PastImmunizationStatusReason StatusReason { get; set; }
    }
}

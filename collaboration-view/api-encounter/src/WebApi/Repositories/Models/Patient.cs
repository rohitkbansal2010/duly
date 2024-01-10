// <copyright file="Patient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Information about an individual receiving health care services.
    /// </summary>
    internal class Patient : IEntityWithIdentifiers
    {
        /// <summary>
        /// General information about a patient.
        /// </summary>
        public PatientGeneralInfo PatientGeneralInfo { get; set; }

        /// <summary>
        /// Birth date of the patient.
        /// </summary>
        public DateTimeOffset BirthDate { get; set; }

        /// <summary>
        /// Birth gender of the patient.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Identifiers of the patient. Format: (Text|VALUE).
        /// </summary>
        public string[] Identifiers { get; set; }

        /// <summary>
        /// Identifiers of the patient. Format: (Text|VALUE).
        /// </summary>
        public List<PatientAddress> PatientAddress { get; set; }

        /// <summary>
        /// List of phone number.
        /// </summary>
        public List<PhoneNumber> PhoneNumber { get; set; }
    }
}

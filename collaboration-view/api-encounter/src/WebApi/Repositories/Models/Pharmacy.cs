// <copyright file="Pharmacy.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Record of preferred pharmacy being taken by a patient.
    /// </summary>
    internal class Pharmacy
    {
        /// <summary>
        /// Pharmacy Name for this pharmacy.
        /// </summary>
        public string PharmacyName { get; set; }

        /// <summary>
        /// AddressLine1 for this pharmacy.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// AddressLine2 for this pharmacy.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// State for this pharmacy.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// PhoneNumber for this pharmacy.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// ClosingTime for this pharmacy.
        /// </summary>
        public string ClosingTime { get; set; }

        /// <summary>
        /// ZipCode of the  pharmacy.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Pharmacy Id of the  pharmacy.
        /// </summary>
        public int PharmacyID { get; set; }

        /// <summary>
        /// City of the  pharmacy.
        /// </summary>
        public string City { get; set; }
    }
}

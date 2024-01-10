// -----------------------------------------------------------------------
// <copyright file="PatientAddress.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Address of patient.
    /// </summary>
    internal class PatientAddress
    {
        /// <summary>
        /// Use .
        /// </summary>
        public string Use { get; set; }

        /// <summary>
        /// Street address .
        /// </summary>
        public IEnumerable<string> Line { get; set; }

        /// <summary>
        /// City .
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Code for mail service .
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Part of Country .
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Country .
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Part of Country .
        /// </summary>
        public string District { get; set; }
    }
}

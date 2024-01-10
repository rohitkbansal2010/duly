// <copyright file="Pharmacy.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Models
{
    public class Pharmacy
    {
        /// <summary>
        /// Name of the Pharmacy.
        /// </summary>
        public string PharmacyName { get; set; }

        /// <summary>
        /// ID of the Pharmacy.
        /// </summary>
        public int PharmacyID { get; set; }

        /// <summary>
        /// AddressLine1 of the Pharmacy.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// AddressLine2 of the Pharmacy.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// State of the Pharmacy.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Phone Number of the Pharmacy.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Closing Time of the Pharmacy.
        /// </summary>
        public string ClosingTime { get; set; }

        /// <summary>
        /// ZipCode of the Pharmacy.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// City of the Pharmacy.
        /// </summary>
        public string City { get; set; }
    }
}

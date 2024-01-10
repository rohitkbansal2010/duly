﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Ngdp.Contracts
{
    public class Pharmacy
    {
        [Description("Name of the Pharmacy")]
        public string PharmacyName { get; set; }

        [Description("Id of the Pharmacy")]
        public int PharmacyID { get; set; }
        [Description("Address 1 of the Pharmacy")]
        public string AddressLine1 { get; set; }
        [Description("Address 2 of the Pharmacy")]
        public string AddressLine2 { get; set; }
        [Description("State of the Pharmacy")]
        public string State { get; set; }
        [Description("Phone Number of the Pharmacy")]
        public string PhoneNumber { get; set; }
        [Description("Closing time of the Pharmacy")]
        public string ClosingTime { get; set; }
        [Description("Zip Code of the Pharmacy")]

        public string ZipCode { get; set; }
        [Description("City of the Pharmacy")]
        public string City { get; set; }
    }
}

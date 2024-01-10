// <copyright file="PatientAddress.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Address")]
    public class PatientAddress
    {
        [Description("use")]
        public string Use { get; set; }

        [Description("Street address")]
        public IEnumerable<string> Line { get; set; }

        [Description("City")]
        public string City { get; set; }

        [Description("Code for mail service")]
        public string PostalCode { get; set; }

        [Description("Part of country")]
        public string State { get; set; }

        [Description("Country")]
        public string Country { get; set; }

        [Description("Part of county")]
        public string District { get; set; }
    }
}

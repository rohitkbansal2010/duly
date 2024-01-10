// <copyright file="Address.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Address")]
    public class Address
    {
        [Description("Street address")]
        [Required]
        public string[] StreetAddress { get; set; }

        [Description("City")]
        public string City { get; set; }

        [Description("Code for mail service")]
        public string PostalCode { get; set; }

        [Description("House number")]
        public string HouseNumber { get; set; }

        [Description("Part of country")]
        public State State { get; set; }

        [Description("Country")]
        public Country Country { get; set; }

        [Description("Part of county")]
        public District District { get; set; }

        [Description("Part of state")]
        public County County { get; set; }
    }
}

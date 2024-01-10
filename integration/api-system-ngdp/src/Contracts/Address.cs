// <copyright file="Address.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("An address expressed using postal conventions (as opposed to GPS or other location definition formats)")]
    public class Address
    {
        [Description("Name of city, town etc.")]
        [Required]
        public string City { get; set; }

        [Description("Sub-unit of country (abbreviations ok)")]
        [Required]
        public string State { get; set; }

        [Description("Postal code for area")]
        public string PostalCode { get; set; }

        //,[Location_Addr_1] ,[Location_Addr_2]
        [Description("Street name, number, direction & P.O. Box etc. This repeating element order: The order in which lines should appear in an address label")]
        public string[] Lines { get; set; }
    }
}
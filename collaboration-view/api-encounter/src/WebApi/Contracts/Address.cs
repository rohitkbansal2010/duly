// <copyright file="Address.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents postal address in U.S.A.")]
    public class Address
    {
        [Description("Address line")]
        public string Line { get; set; }

        [Description("City")]
        public string City { get; set; }

        [Description("State of U.S.A.")]
        public string State { get; set; }

        [Description("Postal code, ZIP code")]
        public string PostalCode { get; set; }
    }
}
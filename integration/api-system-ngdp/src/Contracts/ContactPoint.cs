// <copyright file="ContactPoint.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Details of a Technology mediated contact point (phone, fax, email, etc.)")]
    public class ContactPoint
    {
        [Description("Contact details (telephone, email, etc.) for a contact")]
        [Required]
        public ContactPointType Type { get; set; }

        [Description("The actual contact point details")]
        [Required]
        public string Value { get; set; }
    }
}
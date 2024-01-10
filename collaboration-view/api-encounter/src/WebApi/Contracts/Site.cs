// <copyright file="Site.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about a site")]
    public class Site
    {
        [Description("Site Id")]
        [Required]
        public string Id { get; set; }

        [Description("Site Address")]
        public Address Address { get; set; }
    }
}

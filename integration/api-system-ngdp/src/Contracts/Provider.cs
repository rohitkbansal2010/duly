// <copyright file="Provider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("A person who is involved in the provisioning of healthcare")]
    public class Provider
    {
        [Description("Identifier by which this provider is known")]
        [Required]
        public Identifier Identifier { get; set; }

        [Description("This value includes provider qualification")]
        [Required]
        public string Specialty { get; set; }

        [Description("Practitioner name")]
        [Required]
        public string Name { get; set; }

        [Description("Determs if the provider has avaliable slots in the next two weeks or not")]
        [Required]
        public bool IsSlotAvailableInNext2Weeks { get; set; }
    }
}
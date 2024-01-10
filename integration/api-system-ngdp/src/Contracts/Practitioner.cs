// <copyright file="Practitioner.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Security.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("A person who is directly or indirectly involved in the provisioning of healthcare")]
    public class Practitioner
    {
        [Description("Identifier by which this practitioner is known")]
        [Required]
        [Identity]
        public string Id { get; set; }

        [Description("Practitioner name.")]
        [Required]
        public HumanName HumanName { get; set; }
    }
}

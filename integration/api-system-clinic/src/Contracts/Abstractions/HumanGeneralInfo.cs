// <copyright file="HumanGeneralInfo.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Security.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts.Abstractions
{
    public abstract class HumanGeneralInfo
    {
        [Description("A human identifier.")]
        [Required]
        [Identity]
        public string Id { get; set; }

        [Description("A name associated with the human.")]
        [Required]
        [MinLength(1)]
        public HumanName[] Names { get; set; }
    }
}
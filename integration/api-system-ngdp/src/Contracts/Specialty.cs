// <copyright file="Specialty.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Specialty of recommended provider")]
    public class Specialty
    {
        [Description("Identifier of the specialty")]
        [Required]
        public Identifier Identifier { get; set; }

        [Description("Human readable name")]
        [Required]
        public string Name { get; set; }
    }
}
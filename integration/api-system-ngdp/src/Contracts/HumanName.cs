// <copyright file="HumanName.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Parts of a human name")]
    public class HumanName
    {
        [Description("Family name (often called 'Surname')")]
        [Required]
        public string FamilyName { get; set; }

        [Description("Given names (not always 'first'). Includes middle names. This repeating element order: Given Names appear in the correct order for presenting the name")]
        public string[] GivenNames { get; set; }
    }
}

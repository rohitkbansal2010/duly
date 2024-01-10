// -----------------------------------------------------------------------
// <copyright file="HumanName.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Contracts.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Name of a human - parts and usage")]
    public class HumanName : IDulyResource
    {
        [Description("Family name (often called 'Surname')")]
        public string FamilyName { get; set; }

        [Description("Given names (not always 'first'). Includes middle names. This repeating element order: Given Names appear in the correct order for presenting the name")]
        public string[] GivenNames { get; set; }

        [Description("Parts that come before the name. This repeating element order: Prefixes appear in the correct order for presenting the name")]
        public string[] Prefixes { get; set; }

        [Description("Parts that come after the name. This repeating element order: Suffixes appear in the correct order for presenting the name")]
        public string[] Suffixes { get; set; }

        [Description("The purpose of the name.")]
        [Required]
        public NameUse Use { get; set; }
    }
}
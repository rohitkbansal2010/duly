// <copyright file="Dose.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about the dose of a substance.")]
    public class Dose
    {
        [Description("The amount of a substance.")]
        [Required]
        public decimal Amount { get; set; }

        [Description("Unit of measure for amount of a substance.")]
        public string Unit { get; set; }
    }
}
// <copyright file="Immunization.Status.Reason.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Explains why immunization did not occur")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    public class ImmunizationStatusReason
    {
        [Description("Readable text with explanation")]
        [Required]
        public string Reason { get; set; }
    }
}
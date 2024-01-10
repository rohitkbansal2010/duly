// <copyright file="Visit.Type.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Description of visit type")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    public class VisitType
    {
        [Description("Identifier of the visit type")]
        [Required]
        public Identifier Identifier { get; set; }

        [Description("Human readable name")]
        public string Name { get; set; }
    }
}
// <copyright file="Organization.GeneralInfo.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Security.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    [Description("A grouping of people or organizations with a common purpose")]
    public class OrganizationGeneralInfo
    {
        [Required]
        [Identity]
        [Description("Identifies this organization across multiple systems")]
        public string Id { get; set; }
    }
}
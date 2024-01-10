// -----------------------------------------------------------------------
// <copyright file="Role.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Contracts.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Role which a practitioner may perform")]
    public class Role : IDulyResource
    {
        [Required]
        [Description("String representation of the Role")]
        public string Title { get; set; }
    }
}
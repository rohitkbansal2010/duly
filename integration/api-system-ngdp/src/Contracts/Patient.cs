// <copyright file="Patient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Security.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Patient with details")]
    public class Patient
    {
        [Description("Identifier by which this patient is known")]
        [Required]
        [Identity]
        public string Id { get; set; }
    }
}

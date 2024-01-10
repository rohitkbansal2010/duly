// <copyright file="Identifier.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("An identifier intended for computation")]
    public class Identifier
    {
        [Description("Description of identifier")]
        [Required]
        public string Id { get; set; }

        [Description("The namespace for the identifier value")]
        [Required]
        public IdentifierType Type { get; set; }
    }
}
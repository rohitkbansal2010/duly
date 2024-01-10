// <copyright file="ReferredProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Provider who is referred")]
    public class ReferredProvider
    {
        [Description("Identifier of the referred provider")]
        [Required]
        public Identifier Identifier { get; set; }
    }
}
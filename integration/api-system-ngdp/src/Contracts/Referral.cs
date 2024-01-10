// <copyright file="Referral.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Referral information")]
    public class Referral
    {
        [Description("Identifier of the referral")]
        [Required]
        public Identifier Identifier { get; set; }
    }
}
// <copyright file="Visit.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Information of the visit")]
    public class Visit
    {
        [Description("Visit type")]
        [Required]
        public VisitType Type { get; set; }
    }
}
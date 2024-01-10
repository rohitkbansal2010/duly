// <copyright file="RecommendedProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("Provider who is recommended for the referral")]
    public class RecommendedProvider
    {
        [Description("Identifier of the referral recommended provider. '{Referral_Id}_{Pat_Id}_{Referred_Provider_Id}_{Provider_Id}'")]
        [Required]
        public string Identifier { get; set; }

        [Description("Information about the referal which is originator of the recommended provider")]
        [Required]
        public Referral Referral { get; set; }

        [Description("Specialty of the referral appointment")]
        [Required]
        public Specialty Specialty { get; set; }

        [Description("Preselected provider")]
        public ReferredProvider ReferredProvider { get; set; }

        [Description("Information about department of the provider")]
        [Required]
        public Department Department { get; set; }

        [Description("Location where the original procedure can take place")]
        [Required]
        public Location Location { get; set; }

        [Description("Information about the visit")]
        [Required]
        public Visit Visit { get; set; }

        [Description("Information about the provider")]
        [Required]
        public Provider Provider { get; set; }
    }
}

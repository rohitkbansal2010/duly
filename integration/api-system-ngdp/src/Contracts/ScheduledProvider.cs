// <copyright file="ScheduledProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Ngdp.Contracts
{
    [Description("A person who is involved in the provisioning of healthcare")]
    public class ScheduledProvider
    {
        [Description("Identifier by which this provider is known")]
        [Required]
        public Identifier Identifier { get; init; }

        [Description("Practitioner name")]
        public string Name { get; init; }

        [Description("Provider's photo url")]
        public string PhotoURL { get; init; }
    }
}
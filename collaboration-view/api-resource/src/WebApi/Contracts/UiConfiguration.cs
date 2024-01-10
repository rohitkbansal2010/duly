// -----------------------------------------------------------------------
// <copyright file="UiConfiguration.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Resource.Api.Contracts
{
    [Description("User interface configuration. It is an abstract base class and should not be instantiated.")]
    public class UiConfiguration
    {
        [Description("Configuration identity.")]
        [Required]
        public string Id { get; set; }

        [Description("UI configuration target area type.")]
        [Required]
        public UiConfigurationTargetAreaType TargetAreaType { get; set; }

        [Description("Array of tags.")]
        public string[] Tags { get; set; }
    }
}
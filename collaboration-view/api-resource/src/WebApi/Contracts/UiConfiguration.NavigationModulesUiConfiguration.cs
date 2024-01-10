// <copyright file="UiConfiguration.NavigationModulesUiConfiguration.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Resource.Api.Contracts
{
    [Description("UI configuration for navigating the mudule area.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    public class NavigationModulesUiConfiguration : UiConfiguration
    {
        [Description("Details of a UI configuration for modules navigation.")]
        [Required]
        public NavigationModulesDetails Details { get; set; }
    }
}

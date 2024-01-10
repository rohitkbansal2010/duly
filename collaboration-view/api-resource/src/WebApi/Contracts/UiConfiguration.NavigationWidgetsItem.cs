// <copyright file="UiConfiguration.NavigationWidgetsItem.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Resource.Api.Contracts
{
    [Description("Widget navigation configuration item.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    public class NavigationWidgetsItem
    {
        [Description("Configuration alias.")]
        [Required]
        public string Alias { get; set; }
    }
}

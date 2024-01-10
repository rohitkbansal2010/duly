// -----------------------------------------------------------------------
// <copyright file="UiConfiguration.TargetAreaType.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Resource.Api.Contracts
{
    [Description("User interface configuration area types.")]
    public enum UiConfigurationTargetAreaType
    {
        [Description("For navigation related user interface configurations.")]
        Navigation,

        [Description("For layout related user interface configurations.")]
        Layout
    }
}
// <copyright file="UiConfiguration.WithChildren.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.UiConfig.Adapter.Contracts
{
    /// <summary>
    /// An User interface configuration with all Children configurations.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    public class UiConfigurationWithChildren
    {
        /// <summary>
        /// A user interface configuration that doesn't depend on any other parent configuration.
        /// </summary>
        public UiConfiguration Configuration { get; set; }

        /// <summary>
        /// An array of user interface configurations to which this configuration is the parent.
        /// </summary>
        public UiConfiguration[] Children { get; set; }
    }
}

// <copyright file="UiConfiguration.Item.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.UiConfig.Adapter.Contracts
{
    /// <summary>
    /// User interface configuration item.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    public class UiConfigurationItem
    {
        /// <summary>
        /// An instance of <see cref="UiResource"/> as the item target.
        /// </summary>
        public UiResource ItemTarget { get; set; }

        /// <summary>
        /// Index of the item.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// A UI configuration item target type <see cref="UiConfigurationItemTargetType"/>.
        /// </summary>
        public UiConfigurationItemTargetType ItemTargetType { get; set; }

        /// <summary>
        /// Represents the details as a json string to be converted to the proper object, depending on the ItemTargetType.
        /// </summary>
        public string Details { get; set; }
    }
}
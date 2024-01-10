// <copyright file="UiConfiguration.SearchCriteria.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.UiConfig.Adapter.Contracts
{
    /// <summary>
    /// Parameters for search parent UI configurations (<see cref="UiConfigurationWithChildren"/>).
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    public class UiConfigurationSearchCriteria
    {
        /// <summary>
        /// An application part <see cref="ApplicationPart"/>.
        /// </summary>
        public ApplicationPart ApplicationPart { get; set; }

        /// <summary>
        /// An Id of the site for which you want to find all parent UI configurations.
        /// </summary>
        public string SiteId { get; set; }

        /// <summary>
        /// An Id of a Patient for which you want to find all parent UI configurations.
        /// </summary>
        public string PatientId { get; set; }

        /// <summary>
        /// A UI configuration area type <see cref="UiConfigurationTargetAreaType"/>.
        /// </summary>
        public UiConfigurationTargetAreaType? TargetAreaType { get; set; }
    }
}
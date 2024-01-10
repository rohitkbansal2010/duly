// <copyright file="IConfigurationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Resource.Api.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Resource.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on the corresponding contract for the <see cref="UiConfiguration"/> entity.
    /// </summary>
    public interface IConfigurationRepository
    {
        /// <summary>
        /// Gets UI configurations with nested sub configurations.
        /// </summary>
        /// <param name="applicationPart">An application part <see cref="ApplicationPart"/>.</param>
        /// <param name="siteId">An Id of the site for which you want to find all parent UI configurations.</param>
        /// <param name="patientId">An Id of a Patient for which you want to find all parent UI configurations.</param>
        /// <param name="targetAreaType">A UI configuration area type <see cref="UiConfigurationTargetAreaType"/>.</param>
        /// <returns>All UI configurations that satisfies the parameters.
        /// Now only array of (<see cref="NavigationModulesUiConfiguration"/>).</returns>
        Task<IEnumerable<UiConfiguration>> GetConfigurationsAsync(
            ApplicationPart applicationPart,
            string siteId,
            string patientId,
            UiConfigurationTargetAreaType? targetAreaType = null);
    }
}

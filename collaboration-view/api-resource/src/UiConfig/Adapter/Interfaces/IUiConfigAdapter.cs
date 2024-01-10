// <copyright file="IUiConfigAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.UiConfig.Adapter.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.UiConfig.Adapter.Interfaces
{
    /// <summary>
    /// An adapter over a set of "UiConfig" database stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IUiConfigAdapter
    {
        /// <summary>
        /// Gets parent UI configurations with their child configurations.
        /// </summary>
        /// <param name="criteria">Search criteria.</param>
        /// <returns>All parent UI configurations (<see cref="UiConfigurationWithChildren"/>) that satisfies search criteria.</returns>
        Task<IEnumerable<UiConfigurationWithChildren>> GetConfigurationsAsync(UiConfigurationSearchCriteria criteria);
    }
}

// <copyright file="ISitesRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.Site"/> model.
    /// </summary>
    public interface ISitesRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Models.Site"/> that match the filtering parameters.
        /// </summary>
        /// <returns>Filtered items of <see cref="Models.Site"/>.</returns>
        Task<IEnumerable<Models.Site>> FindSitesAsync();
    }
}

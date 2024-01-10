// -----------------------------------------------------------------------
// <copyright file="ISiteService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// Service for working on <see cref="Site"/> entities.
    /// </summary>
    public interface ISiteService
    {
        /// <summary>
        /// Retrieve all items of <see cref="Site"/>.
        /// </summary>
        /// <returns>Items of <see cref="Site"/>.</returns>
        Task<IEnumerable<Site>> GetSitesAsync();

        /// <summary>
        /// Retrieve <see cref="Site"/> by Id.
        /// </summary>
        /// <param name="siteId">Identifier of the <see cref="Site"/>.</param>
        /// <returns><see cref="Site"/>.</returns>
        Task<Site> GetSiteAsync(string siteId);
    }
}

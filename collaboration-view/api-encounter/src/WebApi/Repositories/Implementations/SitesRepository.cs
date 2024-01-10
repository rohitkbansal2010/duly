// -----------------------------------------------------------------------
// <copyright file="SitesRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// --------------

using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Api.Client;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ISitesRepository"/>
    /// </summary>
    public class SitesRepository : ISitesRepository
    {
        private const string GetSites = "[dbo].[GetSites]";

        private readonly IDapperContext _dapperContext;

        public SitesRepository(IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<Site>> FindSitesAsync()
        {
            return await _dapperContext.QueryAsync<Site>(GetSites);
        }
    }
}
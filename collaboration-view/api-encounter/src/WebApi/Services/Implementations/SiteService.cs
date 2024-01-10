// -----------------------------------------------------------------------
// <copyright file="SiteService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Configurations;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ISiteService"/>
    /// </summary>
    internal class SiteService : ISiteService
    {
        private readonly SiteDataOptions _siteDataOptions;
        private readonly ISitesRepository _repository;
        private readonly IMapper _mapper;

        public SiteService(IOptionsMonitor<SiteDataOptions> optionsMonitor, ISitesRepository repository, IMapper mapper)
        {
            _siteDataOptions = optionsMonitor.CurrentValue;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<Site>> GetSitesAsync()
        {
            var sites = await _repository.FindSitesAsync();
            var result = _mapper.Map<IEnumerable<Site>>(sites);
            return result;

            //return await Task.FromResult(JsonConvert.DeserializeObject<Site[]>(_siteDataOptions.Sites));
        }

        public async Task<Site> GetSiteAsync(string siteId)
        {
            return (await GetSitesAsync()).SingleOrDefault(x => x.Id == siteId);
        }
    }
}

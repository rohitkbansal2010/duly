// <copyright file="ConfigurationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Resource.Api.Contracts;
using Duly.CollaborationView.Resource.Api.Repositories.Interfaces;
using Duly.UiConfig.Adapter.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdapterContracts = Duly.UiConfig.Adapter.Contracts;

namespace Duly.CollaborationView.Resource.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IConfigurationRepository"/>
    /// </summary>
    internal class ConfigurationRepository : IConfigurationRepository
    {
        private readonly IUiConfigAdapter _uiConfigAdapter;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRepository"/> class.
        /// </summary>
        /// <param name="mapper">An instance of mapper.</param>
        /// <param name="uiConfigAdapter">An instance of <see cref="IUiConfigAdapter"/>.</param>
        public ConfigurationRepository(
            IMapper mapper,
            IUiConfigAdapter uiConfigAdapter)
        {
            _mapper = mapper;
            _uiConfigAdapter = uiConfigAdapter;
        }

        public async Task<IEnumerable<UiConfiguration>> GetConfigurationsAsync(
            ApplicationPart applicationPart,
            string siteId,
            string patientId,
            UiConfigurationTargetAreaType? targetAreaType = null)
        {
            var searchCriteria = new AdapterContracts.UiConfigurationSearchCriteria
            {
                ApplicationPart = _mapper.Map<AdapterContracts.ApplicationPart>(applicationPart),
                SiteId = siteId,
                PatientId = patientId,
                TargetAreaType = targetAreaType.HasValue ? _mapper.Map<AdapterContracts.UiConfigurationTargetAreaType>(targetAreaType.Value) : null
            };

            var configurationsWithChildren = await _uiConfigAdapter.GetConfigurationsAsync(searchCriteria);

            return _mapper.Map<IEnumerable<UiConfiguration>>(configurationsWithChildren);
        }
    }
}

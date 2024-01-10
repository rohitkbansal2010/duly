// -----------------------------------------------------------------------
// <copyright file="CvxCodeRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.CdcClient.Interfaces;
using Duly.CollaborationView.CdcClient.Models;
using Duly.CollaborationView.Encounter.Api.Configurations;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.Common.Cache.Clients.Interfaces;
using Duly.Common.Cache.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ICvxCodeRepository"/>
    /// </summary>
    internal class CvxCodeRepository : ICvxCodeRepository
    {
        private const string CdcApiClientErrorPrefix = "CDC REST API client exited with an error:";
        private const string RedisCacheClientErrorPrefix = "The Redis Cache client exited with an error:";
        private const string RedisCacheClientAddWarningPrefix = "Redis Cache client failed to store the value for key: ";

        private const string CvxCodeAlias = "CvxCode";
        private const string RelationshipMnemonic = "IS_MEMBER_OF";

        private readonly ICdcApiClient _cdcApiClient;
        private readonly IOptionsMonitor<CdcApiOptions> _cdcApiOptionsMonitor;
        private readonly ICacheKeyBuilder _cacheKeyBuilder;
        private readonly ICacheClient _cacheClient;
        private readonly ILogger<CvxCodeRepository> _logger;

        public CvxCodeRepository(
            ICdcApiClient cdcApiClient,
            IOptionsMonitor<CdcApiOptions> cdcApiOptionsMonitor,
            ICacheKeyBuilder cacheKeyBuilder,
            ICacheClient cacheClient,
            ILogger<CvxCodeRepository> logger)
        {
            _cdcApiClient = cdcApiClient;
            _cdcApiOptionsMonitor = cdcApiOptionsMonitor;
            _cacheKeyBuilder = cacheKeyBuilder;
            _cacheClient = cacheClient;
            _logger = logger;
        }

        private string ContentModel => _cdcApiOptionsMonitor.CurrentValue.ContentModel;
        private string CatalogCVX => _cdcApiOptionsMonitor.CurrentValue.CatalogCVX;
        private string CatalogVaccineGroup => _cdcApiOptionsMonitor.CurrentValue.CatalogVaccineGroup;

        public async Task<IReadOnlyDictionary<string, string>> FindVaccineGroupNamesByCodesAsync(string[] codes)
        {
            if (codes == null || codes.Length == 0)
                return new Dictionary<string, string>(0);

            var uncachedCodes = new List<string>();
            var cvxCodeVaccineGroupNameDictionary = await BuildCvxCodeVaccineGroupNameDictionaryFromCacheAsync(codes, uncachedCodes);

            if (!uncachedCodes.Any())
                return cvxCodeVaccineGroupNameDictionary;

            return await FillCvxCodeVaccineGroupNameDictionaryFromCdcApiAsync(uncachedCodes, cvxCodeVaccineGroupNameDictionary);
        }

        private async Task<IReadOnlyDictionary<string, string>> FillCvxCodeVaccineGroupNameDictionaryFromCdcApiAsync(
            IEnumerable<string> uncachedCodes,
            Dictionary<string, string> cvxCodeVaccineGroupNameDictionary)
        {
            var immunizationCdcCatalogs = await GetImmunizationCdcCatalogsAsync();
            if (immunizationCdcCatalogs == null)
                return cvxCodeVaccineGroupNameDictionary;

            var foundRelations =
                await CatalogRelationsByCodesAsync(immunizationCdcCatalogs.CvxCatalogUID, uncachedCodes.ToArray());
            foreach (var foundRelation in foundRelations)
            {
                var cvxCode = foundRelation.RequestedTerm.TermSourceCode;
                var vaccineGroupName = foundRelation.RelatedItems
                    .FirstOrDefault(x => x.CatalogUID == immunizationCdcCatalogs.VaccineGroupCatalogUID
                                         && x.RelationshipMnemonic == RelationshipMnemonic)
                    ?.RelatedTerm
                    ?.TermDescription;

                if (vaccineGroupName == null)
                    continue;

                if (!await AddVaccineGroupNameToCacheByCvxCodeAsync(cvxCode, vaccineGroupName))
                {
                    _logger.LogWarning($"{RedisCacheClientAddWarningPrefix}[{_cacheKeyBuilder.GetCacheKey(CvxCodeAlias, cvxCode)}].");
                }

                cvxCodeVaccineGroupNameDictionary.Add(cvxCode, vaccineGroupName);
            }

            return cvxCodeVaccineGroupNameDictionary;
        }

        private async Task<CdcCatalogSourceCodeRelations[]> CatalogRelationsByCodesAsync(
            string cvxCatalogUID,
            string[] cvxCodes)
        {
            try
            {
                return await _cdcApiClient
                    .FindContentModelCatalogRelationsByCodesAsync(
                        ContentModel,
                        cvxCatalogUID,
                        cvxCodes);
            }
            catch (Exception e)
            {
                _logger.LogError($"{CdcApiClientErrorPrefix} {e.Message}");
            }

            return Array.Empty<CdcCatalogSourceCodeRelations>();
        }

        private async Task<Dictionary<string, string>> BuildCvxCodeVaccineGroupNameDictionaryFromCacheAsync(
            IEnumerable<string> codes,
            ICollection<string> uncachedCodes)
        {
            var cvxCodeVaccineGroupNameDictionary = new Dictionary<string, string>();

            foreach (var cvxCode in codes)
            {
                var vaccineGroupName = await GetCachedVaccineGroupNameAsync(cvxCode);

                if (vaccineGroupName == null)
                {
                    uncachedCodes.Add(cvxCode);
                    continue;
                }

                cvxCodeVaccineGroupNameDictionary.Add(cvxCode, vaccineGroupName);
            }

            return cvxCodeVaccineGroupNameDictionary;
        }

        private async Task<string> GetCachedVaccineGroupNameAsync(string cvxCode)
        {
            var vaccineGroupName = default(string);
            try
            {
                vaccineGroupName = await _cacheClient.GetAsync<string>(_cacheKeyBuilder.GetCacheKey(CvxCodeAlias, cvxCode));
            }
            catch (Exception e)
            {
                _logger.LogError($"{RedisCacheClientErrorPrefix} {e.Message}");
            }

            return vaccineGroupName;
        }

        private async Task<bool> AddVaccineGroupNameToCacheByCvxCodeAsync(string cvxCode, string vaccineGroupName)
        {
            try
            {
               return await _cacheClient.AddAsync(
                    _cacheKeyBuilder.GetCacheKey(CvxCodeAlias, cvxCode),
                    vaccineGroupName);
            }
            catch (Exception e)
            {
                _logger.LogError($"{RedisCacheClientErrorPrefix} {e.Message}");
            }

            return false;
        }

        private async Task<ImmunizationCdcCatalogs> GetImmunizationCdcCatalogsAsync()
        {
            var cacheKeyImmunizationCdcCatalogs =
                _cacheKeyBuilder.GetCacheKey(nameof(CvxCodeRepository), nameof(ImmunizationCdcCatalogs));

            var immunizationCdcCatalogs = await GetCachedImmunizationCdcCatalogsAsync(cacheKeyImmunizationCdcCatalogs);

            if (immunizationCdcCatalogs != null)
                return immunizationCdcCatalogs;

            var catalogs = await GetContentModelCatalogsAsync();
            var cvxCatalogUID = catalogs
                ?.FirstOrDefault(x => x.Mnemonic == CatalogCVX)
                ?.CatalogUID;
            var vaccineGroupCatalogUID = catalogs
                ?.FirstOrDefault(x => x.Mnemonic == CatalogVaccineGroup)
                ?.CatalogUID;
            if (cvxCatalogUID == null || vaccineGroupCatalogUID == null)
            {
                _logger.LogWarning($"CDC REST API client does not find the required catalogs.");
                return null;
            }

            immunizationCdcCatalogs = new ImmunizationCdcCatalogs
            {
                CvxCatalogUID = cvxCatalogUID,
                VaccineGroupCatalogUID = vaccineGroupCatalogUID
            };

            if (!await AddVaccineGroupNameToCacheByCvxCodeAsync(cacheKeyImmunizationCdcCatalogs, immunizationCdcCatalogs))
            {
                _logger.LogWarning($"{RedisCacheClientAddWarningPrefix}[{cacheKeyImmunizationCdcCatalogs}].");
            }

            return immunizationCdcCatalogs;
        }

        private async Task<CdcContentModelCatalog[]> GetContentModelCatalogsAsync()
        {
            try
            {
                return await _cdcApiClient.GetContentModelCatalogsAsync(ContentModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"{CdcApiClientErrorPrefix} {e.Message}");
            }

            return null;
        }

        private async Task<ImmunizationCdcCatalogs> GetCachedImmunizationCdcCatalogsAsync(string cacheKeyImmunizationCdcCatalogs)
        {
            try
            {
               return await _cacheClient.GetAsync<ImmunizationCdcCatalogs>(cacheKeyImmunizationCdcCatalogs);
            }
            catch (Exception e)
            {
                _logger.LogError($"{RedisCacheClientErrorPrefix} {e.Message}");
            }

            return null;
        }

        private async Task<bool> AddVaccineGroupNameToCacheByCvxCodeAsync(
            string cacheKeyImmunizationCdcCatalogs,
            ImmunizationCdcCatalogs immunizationCdcCatalogs)
        {
            try
            {
                return await _cacheClient.AddAsync(cacheKeyImmunizationCdcCatalogs, immunizationCdcCatalogs, null);
            }
            catch (Exception e)
            {
                _logger.LogError($"{RedisCacheClientErrorPrefix} {e.Message}");
            }

            return false;
        }
    }
}

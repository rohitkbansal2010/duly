// <copyright file="UiConfigAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Dapper;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.UiConfig.Adapter.Contracts;
using Duly.UiConfig.Adapter.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Duly.UiConfig.Adapter.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IUiConfigAdapter"/>
    /// </summary>
    public class UiConfigAdapter : IUiConfigAdapter
    {
        private const string FindConfigurationsStoredProcedureName = "[core].[uspConfigurationsSelect]";
        private const string ApplicationPartParameterName = "@AppPart";
        private const string SiteIdParameterName = "@SiteId";
        private const string PatientIdParameterName = "@PatientId";
        private const string TargetAreaTypeParameterName = "@TargetAreaType";

        private readonly IDapperContext _dapperContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiConfigAdapter"/> class.
        /// </summary>
        /// <param name="dapperContext">An instance of the class that
        /// implements the <see cref="IDapperContext"/> interface for "UiConfig" database.</param>
        public UiConfigAdapter(IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Task<IEnumerable<UiConfigurationWithChildren>> GetConfigurationsAsync(UiConfigurationSearchCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            return GetUiConfigurationWithChildren(criteria);
        }

        private async Task<IEnumerable<UiConfigurationWithChildren>> GetUiConfigurationWithChildren(UiConfigurationSearchCriteria criteria)
        {
            var parameters = new DynamicParameters();
            parameters.Add(ApplicationPartParameterName, criteria.ApplicationPart.ToString(), DbType.String, ParameterDirection.Input);
            parameters.Add(SiteIdParameterName, criteria.SiteId, DbType.String, ParameterDirection.Input);
            parameters.Add(PatientIdParameterName, criteria.PatientId, DbType.String, ParameterDirection.Input);
            parameters.Add(TargetAreaTypeParameterName, criteria.TargetAreaType?.ToString(), DbType.String, ParameterDirection.Input);

            return await _dapperContext.ExecuteJsonResultAsync<UiConfigurationWithChildren>(FindConfigurationsStoredProcedureName, parameters);
        }
    }
}

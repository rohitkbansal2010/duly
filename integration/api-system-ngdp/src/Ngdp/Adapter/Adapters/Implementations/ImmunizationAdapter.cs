// <copyright file="ImmunizationAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IImmunizationAdapter"/>
    /// </summary>
    internal class ImmunizationAdapter : IImmunizationAdapter
    {
        private const string FindImmunizationsStoredProcedureName = Constants.SchemaName + Constants.NameSeparator + "[uspImmunizationsSelectByPatientId]";

        private readonly IDapperContext _dapperContext;

        public ImmunizationAdapter(IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Task<IEnumerable<Immunization>> FindImmunizationsAsync(ImmunizationSearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new ArgumentNullException(nameof(searchCriteria));
            }

            var parameters = searchCriteria.ConvertToParameters();
            return _dapperContext.QueryAsync<Immunization>(FindImmunizationsStoredProcedureName, parameters);
        }
    }
}
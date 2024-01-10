// <copyright file="PharmacyAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPharmacyAdapter"/>
    /// </summary>
    internal class PharmacyAdapter : IPharmacyAdapter
    {
        private const string FindPreferredPharmacyStoredProcedureName = "[uspPatientPreferredPharmacySelect]";

        private readonly ICVDapperContext _dapperContext;

        public PharmacyAdapter(ICVDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<Pharmacy> FindPreferredPharmacyByPatientIdAsync(string patientId)
        {
            dynamic parameters = new { Patient_ID = patientId };
            var result = await _dapperContext.QueryFirstOrDefaultAsync<Pharmacy>(FindPreferredPharmacyStoredProcedureName, parameters);
            return result;
        }
    }
}

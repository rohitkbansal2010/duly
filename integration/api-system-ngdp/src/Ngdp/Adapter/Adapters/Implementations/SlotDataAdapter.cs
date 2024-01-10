// <copyright file="SlotDataAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="ISlotDataAdapter"/>
    /// </summary>
    internal class SlotDataAdapter : ISlotDataAdapter
    {
        private const string FindDepartmentVisitTypeIdProcedureName = "[uspGetDepartmentVisittype]";

        private readonly IDapperContext _dapperContext;

        public SlotDataAdapter(IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<DepartmentVisitType> FindSlotDataAsync(string appointmentId)
        {
            var parameters = new
            {
                appointmentId = appointmentId
            };

            var result = await _dapperContext.QuerySingleOrDefaultAsync<DepartmentVisitType>(FindDepartmentVisitTypeIdProcedureName, parameters);
            return result;
        }
    }
}
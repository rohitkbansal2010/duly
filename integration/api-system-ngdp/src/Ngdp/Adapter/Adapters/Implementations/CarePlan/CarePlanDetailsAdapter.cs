// <copyright file="CarePlanDetailsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan;
using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Implementations.CarePlan
{
    /// <summary>
    /// <inheritdoc cref="ICarePlanDetailsAdapter"/>
    /// </summary>
    internal class CarePlanDetailsAdapter : ICarePlanDetailsAdapter
    {
        private const string GetCarePlanDetailsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetCarePlanForPatient]";

        private readonly ICVDapperContext _dapperContext;

        public CarePlanDetailsAdapter(ICVDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<CarePlanDetails>> GetCarePlanDetailsAsync(string patientId)
        {
            var parameters = new
            {
                PatientId = patientId
            };
            var result = await _dapperContext.QueryAsync<CarePlanDetails>(GetCarePlanDetailsProcedureName, parameters);
            return result;
        }
    }
}
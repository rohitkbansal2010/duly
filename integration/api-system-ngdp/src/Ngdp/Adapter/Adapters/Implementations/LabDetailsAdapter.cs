// <copyright file="LabDetailsAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="ILabDetailsAdapter"/>
    /// </summary>
    internal class LabDetailsAdapter : ILabDetailsAdapter
    {
        private const string InsertLabDetailsStoredProcedureName = "[InsertLabDetails]";
        private const string FindLabLocationByLatLng = "[GetLabLocationByLatLng]";

        private readonly ICVDapperContext _dapperContext;

        public LabDetailsAdapter(ICVDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Task<int> PostLabDetailsAsync(LabDetails request)
        {
            dynamic labDetailParameter = new
            {
                Id = request.ID,
                Type = request.Type,
                Lab_ID = request.Lab_ID,
                Lab_Location = request.Lab_Location,
                Lab_Name = request.Lab_Name,
                Appointment_ID = request.Appointment_ID,
                Patient_ID = request.Patient_ID,
                Skipped = request.Skipped
            };
            return _dapperContext.ExecuteScalarAsync<int>(InsertLabDetailsStoredProcedureName, labDetailParameter);
        }

        public Task<IEnumerable<LabLocation>> FindLabLocationByLatLngAsync(LabLocationSearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new ArgumentNullException(nameof(searchCriteria));
            }

            var parameters = searchCriteria.ConvertToParameters();
            var data = _dapperContext.QueryAsync<LabLocation>(FindLabLocationByLatLng, parameters);

            return data;
        }
    }
}
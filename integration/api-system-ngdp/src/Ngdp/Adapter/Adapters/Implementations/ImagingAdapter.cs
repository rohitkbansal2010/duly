// <copyright file="ImagingAdapter.cs" company="Duly Health and Care">
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
    internal class ImagingAdapter : IImagingAdapter
    {
        private const string InsertImagingStoredProcedureName = "[InsertLabDetails]";

 

        private readonly ICVDapperContext _dapperContext;

        public ImagingAdapter(ICVDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Task<int> ImagingAsync(ImagingDetail request)
        {
            dynamic labDetailParameter = new
            {
                Id=request.ID,
                Type = request.Type,
                ImagingType = request.ImagingType,
                Appointment_ID = request.Appointment_ID,
                Patient_ID = request.Patient_ID,
                Provider_ID = request.Provider_ID,
                Location_ID = request.Location_ID,
                BookingSlot = request.BookingSlot,
                AptScheduleDate = request.AptScheduleDate,
                ImagingLocation = request.ImagingLocation,
                Skipped = request.Skipped
            };
            return _dapperContext.ExecuteScalarAsync<int>(InsertImagingStoredProcedureName, labDetailParameter);
        }
    }
}
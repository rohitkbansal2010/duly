// <copyright file="ScheduleFollowUpAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IScheduleFollowUpAdapter"/>
    /// </summary>
    internal class ScheduleFollowUpAdapter : IScheduleFollowUpAdapter
    {
        private const string PostScheduleFollowUpStoredProcedureName = "[insertScheduleFollowUp]";

        private readonly ICVDapperContext _dapperContext;

        public ScheduleFollowUpAdapter(ICVDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Task<int> PostScheduleFollowUpAsync(Models.ScheduleFollowUp request)
        {
            dynamic ScheduleFollowUpParameter = new
            {
                Id = request.ID,
                Provider_Id = request.Provider_ID,
                Patient_Id = request.Patient_ID,
                AptType = request.AptType,
                AptFormat = request.AptFormat,
                Location_ID = request.Location_ID,
                AptScheduleDate = request.AptScheduleDate,
                BookingSlot = request.BookingSlot,
                RefVisitType = request.RefVisitType,
                Type = request.Type,
                Appointment_Id = request.Appointment_Id,
                Skipped = request.Skipped
            };
            return _dapperContext.ExecuteScalarAsync<int>(PostScheduleFollowUpStoredProcedureName, ScheduleFollowUpParameter);
        }
    }
}
// <copyright file="ReferralDetailAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IReferralDetailAdapter"/>
    /// </summary>
    internal class ReferralDetailAdapter : IReferralDetailAdapter
    {
        private const string PostReferralDetailStoredProcedureName = "[insertScheduleFollowUp]";

        private readonly ICVDapperContext _dapperContext;

        public ReferralDetailAdapter(ICVDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Task<int> PostReferralDetailAsync(Models.ReferralDetail request)
        {
            dynamic ReferralDetailParameter = new
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
                Skipped = request.Skipped,
                DepartmentId = request.Department_Id
            };
            return _dapperContext.ExecuteScalarAsync<int>(PostReferralDetailStoredProcedureName, ReferralDetailParameter);
        }
    }
}
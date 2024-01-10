// <copyright file="CheckOutDetailsAdapter.cs" company="Duly Health and Care">
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
    /// <inheritdoc cref="IImmunizationAdapter"/>
    /// </summary>
    internal class CheckOutDetailsAdapter : ICheckOutDetailsAdapter
    {
        private const string FindLabDetailsProcedureName = "[uspGetLabDetails]";
        private const string FindScheduleFollowUpProcedureName = "[uspGetScheduleFollowUp]";

        private readonly ICVDapperContext _dapperContext;

        public CheckOutDetailsAdapter(ICVDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<CheckOutDetails> FindCheckOutDetailsAsync(string appointmentId)
        {
            var parameters = new
            {
                id = appointmentId
            };

            var checkOutDetails = new CheckOutDetails();
            checkOutDetails.LabDetailsList = await _dapperContext.QueryAsync<GetLabOrImagingDetails>(FindLabDetailsProcedureName, parameters);
            checkOutDetails.ScheduleFollowUpList = await _dapperContext.QueryAsync<ReferralDetail>(FindScheduleFollowUpProcedureName, parameters);
            if(checkOutDetails.LabDetailsList != null && checkOutDetails.ScheduleFollowUpList != null
                && checkOutDetails.LabDetailsList.ToList().Count() == 0 && checkOutDetails.ScheduleFollowUpList.ToList().Count == 0)
            {
                checkOutDetails.Message = "No Record Found for Appointment:" + appointmentId;
            }

            return checkOutDetails;
        }
    }
}
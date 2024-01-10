// -----------------------------------------------------------------------
// <copyright file="CheckOutDetailsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;

using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IPatientRepository"/>
    /// </summary>
    public class CheckOutDetailsRepository : ICheckOutDetailsRepository
    {
        private const string FindLabDetailsProcedureName = "[uspGetLabDetails]";
        private const string FindScheduleFollowUpProcedureName = "[uspGetScheduleFollowUp]";

        private readonly IDapperContext _dapperContext;

        public CheckOutDetailsRepository(
            IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<Models.CheckOut.CheckOutDetails> GetCheckOutDetailsByIdAsync(string appointmentId)
        {
            var parameters = new
            {
                id = appointmentId
            };

            var checkOutDetails = new Models.CheckOut.CheckOutDetails();
            checkOutDetails.LabDetailsList = await _dapperContext.QueryAsync<Models.CheckOut.GetLabOrImaging>(FindLabDetailsProcedureName, parameters);
            checkOutDetails.ScheduleFollowUpList = await _dapperContext.QueryAsync<Models.CheckOut.ScheduleReferral>(FindScheduleFollowUpProcedureName, parameters);
            if (checkOutDetails.LabDetailsList != null && checkOutDetails.ScheduleFollowUpList != null
                && !checkOutDetails.LabDetailsList.Any() && !checkOutDetails.ScheduleFollowUpList.Any())
            {
                checkOutDetails.Message = "No Record Found for Appointment:" + appointmentId;
            }

            return checkOutDetails;
        }
    }
}

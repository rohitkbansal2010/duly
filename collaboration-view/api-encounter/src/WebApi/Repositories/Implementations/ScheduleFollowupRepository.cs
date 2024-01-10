// -----------------------------------------------------------------------
// <copyright file="ScheduleFollowupRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// --------------

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CheckOut;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Api.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IScheduleFollowupRepository"/>
    /// </summary>
    public class ScheduleFollowupRepository : IScheduleFollowupRepository
    {
        private const string PostScheduleFollowUpStoredProcedureName = "[insertScheduleFollowUp]";
        private const string InsertDataPostedToEpicStoredProcedureName = "[UpdateDataPostedToEpic]";

        private readonly IDapperContext _dapperContext;

        public ScheduleFollowupRepository(
            IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> PostScheduleFollowup(Models.CheckOut.ScheduleFollowUp request)
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
                Skipped = request.Skipped,
                VisitType_Id = request.VisitTypeId
            };
            return await _dapperContext.ExecuteScalarAsync<int>(PostScheduleFollowUpStoredProcedureName, ScheduleFollowUpParameter);
         }

        public async Task<int> PostDataPostedToEpic(int id)
        {
            dynamic labDetailParameter = new
            {
                Id = id,
                DataPOstedToEpic = 1
            };
            await _dapperContext.ExecuteScalarAsync(InsertDataPostedToEpicStoredProcedureName, labDetailParameter);
            return id;
        }
    }
}
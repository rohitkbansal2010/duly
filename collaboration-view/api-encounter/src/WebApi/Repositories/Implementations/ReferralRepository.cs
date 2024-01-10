// -----------------------------------------------------------------------
// <copyright file="ReferralRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// --------------

using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Api.Client;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IReferralRepository"/>
    /// </summary>
    public class ReferralRepository : IReferralRepository
    {
        private const string InsertDataPostedToEpicStoredProcedureName = "[UpdateDataPostedToEpic]";
        private const string PostReferralDetailStoredProcedureName = "[insertScheduleFollowUp]";

        private readonly IDapperContext _dapperContext;

        public ReferralRepository(
            IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> PostReferral(Models.CheckOut.ScheduleReferral request)
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

            return await _dapperContext.ExecuteScalarAsync<int>(PostReferralDetailStoredProcedureName, ReferralDetailParameter);
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
// <copyright file="ReferralRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Interfaces;
using Duly.OmniChannel.Orchestrator.Appointment.Common.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Duly.OmniChannel.Orchestrator.Appointment.Common.Repositories
{
    /// <inheritdoc cref="IReferralRepository" />
    public class ReferralRepository : IReferralRepository
    {
        private readonly IDapperContext _dapperContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralRepository" /> class.
        /// </summary>
        /// <param name="dapperContext">An instance of <see cref="IDapperContext" /> class.</param>
        public ReferralRepository(IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ReferralOrder>> GetReferralOrdersForDeliveryAsync(IDbTransaction transaction = null)
        {
            const string spName = "[dulycv].[uspReferralOrderSelect]";

            return await _dapperContext.QueryAsync<ReferralOrder>(spName, null, transaction);
        }

        /// <inheritdoc />
        public async Task<int> UpdateReferralOrdersStatusAsync(string referralId, string status, string meta = null, IDbTransaction transaction = null)
        {
            const string spName = "[dulycv].[uspUpdateStatusByReferralId]";

            var spParameters = new
            {
                ReferralId = referralId,
                Status = status,
                Meta = meta
            };

            return await _dapperContext.ExecuteAsync(spName, spParameters, transaction);
        }

        /// <inheritdoc />
        public async Task<int> StoreAppointmentDetailsAsync(string referralId, Models.Appointment appointment, IDbTransaction transaction = null)
        {
            const string spName = "[dulycv].[uspReferralAppointmentInsert]";

            var spParameters = new
            {
                ReferralId = referralId,
                ApptCSN = appointment.CSN,
                ApptDate = appointment.StartDateTime.Date,
                ApptTime = appointment.StartDateTime.TimeOfDay,
                ApptTimeZone = appointment.TimeZone,
                ApptDurationInMins = appointment.DurationInMinutes,
                appointment.ProviderDisplayName,
                appointment.ProviderExternalId,
                ProviderPhotoURL = appointment.ProviderPhotoUrl,
                appointment.VisitTypeExternalId,
                DeptExternalId = appointment.DepartmentExternalId,
                DeptName = appointment.DepartmentName,
                DeptAddrSt = appointment.DepartmentStreetName,
                DeptCity = appointment.DepartmentCity,
                DeptState = appointment.DepartmentState,
                DeptZip = appointment.DepartmentZipCode,
                ApptScheduledTime = appointment.CreationDateTime
            };

            return await _dapperContext.ExecuteAsync(spName, spParameters, transaction);
        }
    }
}

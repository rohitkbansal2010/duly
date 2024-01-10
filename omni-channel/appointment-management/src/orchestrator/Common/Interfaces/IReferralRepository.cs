// <copyright file="IReferralRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.OmniChannel.Orchestrator.Appointment.Common.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Duly.OmniChannel.Orchestrator.Appointment.Common.Interfaces
{
    /// <summary>
    /// Repository for handling low level database operations with referrals.
    /// </summary>
    public interface IReferralRepository
    {
        /// <summary>
        /// Calls stored procedure to get active referral orders ready for delivery.
        /// </summary>
        /// <param name="transaction">A transaction for stored procedure call.</param>
        /// <returns>
        /// A collection of <see cref="ReferralOrder" /> entities.
        /// </returns>
        Task<IEnumerable<ReferralOrder>> GetReferralOrdersForDeliveryAsync(IDbTransaction transaction = null);

        /// <summary>
        /// Calls stored procedure to update status of the referral order after delivery.
        /// </summary>
        /// <param name="referralId">An unique identifier of the referral order.</param>
        /// <param name="status">An updated status of the referral order.</param>
        /// <param name="meta">An additional metadata about the referral order.</param>
        /// <param name="transaction">A transaction for stored procedure call.</param>
        /// <returns>
        /// A task that represents the asynchronous stored procedure call.
        /// </returns>
        Task<int> UpdateReferralOrdersStatusAsync(string referralId, string status, string meta = null, IDbTransaction transaction = null);

        /// <summary>
        /// Call stored procedure to store appointment details for the referral order.
        /// </summary>
        /// <param name="referralId">An unique identifier of the referral order.</param>
        /// <param name="appointment">A model of appointment details for referral.</param>
        /// <param name="transaction">A transaction for stored procedure call.</param>
        /// <returns>
        /// A task that represents the asynchronous stored procedure call.
        /// </returns>
        Task<int> StoreAppointmentDetailsAsync(string referralId, Models.Appointment appointment, IDbTransaction transaction = null);
    }
}

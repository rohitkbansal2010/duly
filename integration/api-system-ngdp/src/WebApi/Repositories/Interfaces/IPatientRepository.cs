// <copyright file="IPatientRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        /// <summary>
        /// Retrieve <see cref="ReferralPatient"/> which match with the referral id.
        /// </summary>
        /// <param name="referralId">Referral identifier.</param>
        /// <returns>Item of <see cref="ReferralPatient"/>.</returns>
        public Task<ReferralPatient> GetPatientByReferralIdAsync(string referralId);
    }
}
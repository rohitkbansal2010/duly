// <copyright file="ICarePlanDetailsRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts.CarePlan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces.CarePlan
{
    public interface ICarePlanDetailsRepository
    {
        /// <summary>
        /// Get Care Plan Details.
        /// </summary>
        /// <param name="patientId">Patient Identifier.</param>
        /// <returns>List items of <see cref="CarePlanDetails"/>.</returns>
        Task<IEnumerable<CarePlanDetails>> GetCarePlanDetailsAsync(string patientId);
    }
}
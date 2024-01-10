// <copyright file="ICarePlanDetailsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models.CarePlan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces.CarePlan
{
    /// <summary>
    /// An adapter for working with "Care Plan Details" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface ICarePlanDetailsAdapter
    {
        /// <summary>
        /// Get Care Plan Details.
        /// </summary>
        /// <returns>List items of <see cref="CarePlanDetails"/>.</returns>
        Task<IEnumerable<CarePlanDetails>> GetCarePlanDetailsAsync(string patientId);
    }
}
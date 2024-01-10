// <copyright file="ISlotDataRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="DepartmentVisitType"/> entities.
    /// </summary>
    public interface ISlotDataRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="DepartmentVisitType"/> which match with the filter.
        /// </summary>
        /// <param name="appointmentId">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="DepartmentVisitType"/>.</returns>
        Task<DepartmentVisitType> GetSlotDataAsync(string appointmentId);
    }
}
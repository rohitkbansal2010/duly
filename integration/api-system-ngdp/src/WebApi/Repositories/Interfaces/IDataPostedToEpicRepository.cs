// <copyright file="IDataPostedToEpicRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    /// <summary>
    /// Updates Databse if Data is Posted to Epic.
    /// </summary>
    public interface IDataPostedToEpicRepository
    {
        /// <summary>
        /// Save all items of <see cref="LabDetails"/> which match with the filter.
        /// </summary>
        /// <param name="id">Identifier of Data.</param>
        Task<int> DataPostedToEpicAsync(int id);
    }
}
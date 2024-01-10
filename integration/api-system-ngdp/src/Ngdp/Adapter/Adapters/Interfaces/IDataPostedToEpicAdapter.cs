// <copyright file="IDataPostedToEpicAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// An adapter for working with "DataPostedToEpic" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface IDataPostedToEpicAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="LabDetails"/>.
        /// </summary>
        /// <param name="id">Id of Data to be Updated.</param>
        /// <returns>Filtered items of <see cref="LabDetails"/>.</returns>
        Task<int> PostDataPostedToEpicAsync(int id);
    }
}
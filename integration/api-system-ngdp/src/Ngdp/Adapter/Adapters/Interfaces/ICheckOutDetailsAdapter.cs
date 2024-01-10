// <copyright file="ICheckOutDetailsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// An adapter for working with "CheckOutDetails" in ngdp database via stored procedures that performs CRUD operations on data.
    /// </summary>
    public interface ICheckOutDetailsAdapter
    {
        /// <summary>
        /// Finds matching items of <see cref="CheckOutDetails"/> by <see cref="appointmentId"/>.
        /// </summary>
        /// <param name="appointmentId">Criteria to search by.</param>
        /// <returns>Filtered items of <see cref="CheckOutDetails"/>.</returns>
        Task<CheckOutDetails> FindCheckOutDetailsAsync(string appointmentId);
    }
}
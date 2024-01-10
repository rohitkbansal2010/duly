// <copyright file="IEncounterRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Encounter"/> entities.
    /// </summary>
    public interface IEncounterRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Encounter"/> which match with the filter.
        /// </summary>
        /// <param name="siteId">Identifier of clinic.</param>
        /// <param name="date">Filter by date.</param>
        /// <returns>Filtered items of <see cref="Encounter"/>.</returns>
        public Task<IEnumerable<Encounter>> GetEncountersForSiteByDateAsync(string siteId, DateTime date);
    }
}

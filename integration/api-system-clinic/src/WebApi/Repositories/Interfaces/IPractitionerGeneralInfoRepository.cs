// <copyright file="IPractitionerGeneralInfoRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="PractitionerGeneralInfo"/> entities.
    /// </summary>
    public interface IPractitionerGeneralInfoRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="PractitionerGeneralInfo"/> which match with the filter.
        /// </summary>
        /// <param name="siteId">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="PractitionerGeneralInfo"/>.</returns>
        Task<IEnumerable<PractitionerGeneralInfo>> GetPractitionersBySiteIdAsync(string siteId);

        /// <summary>
        /// Returns all available items of <see cref="PractitionerGeneralInfo"/> for a set of identifiers.
        /// </summary>
        /// <param name="identifiers">Identifiers of practitioners.</param>
        /// <returns>Returns practitioners.</returns>
        Task<IEnumerable<PractitionerGeneralInfo>> GetPractitionersByIdentifiersAsync(string[] identifiers);
    }
}
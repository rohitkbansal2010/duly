// <copyright file="IPractitionerRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.PractitionerGeneralInfo"/> model.
    /// </summary>
    internal interface IPractitionerRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Models.PractitionerGeneralInfo"/> that match the filtering parameter.
        /// </summary>
        /// <param name="siteId">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="Models.PractitionerGeneralInfo"/>.</returns>
        Task<IEnumerable<Models.PractitionerGeneralInfo>> GetPractitionersBySiteIdAsync(string siteId);

        /// <summary>
        /// Retrieve all items of <see cref="Models.PractitionerGeneralInfo"/> with specified IDs.
        /// </summary>
        /// <param name="practitionerIds">Practitioners identifiers.</param>
        /// <returns>Filtered items of <see cref="Models.PractitionerGeneralInfo"/>.</returns>
        Task<IEnumerable<Models.PractitionerGeneralInfo>> GetPractitionersByIdsAsync(string[] practitionerIds);
    }
}

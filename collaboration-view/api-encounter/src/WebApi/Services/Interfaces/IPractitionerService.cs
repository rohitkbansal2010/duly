// <copyright file="IPractitionerService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.PractitionerGeneralInfo"/> entity.
    /// </summary>
    public interface IPractitionerService
    {
        /// <summary>
        /// Retrieve all items of <see cref="Contracts.PractitionerGeneralInfo"/> that match the filtering parameter.
        /// </summary>
        /// <param name="siteId">Identifier of clinic.</param>
        /// <returns>Filtered items of <see cref="Contracts.PractitionerGeneralInfo"/>.</returns>
        Task<IEnumerable<Contracts.PractitionerGeneralInfo>> GetPractitionersBySiteIdAsync(string siteId);
    }
}

// <copyright file="IPastImmunizationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="PastImmunization"/> entities.
    /// </summary>
    internal interface IPastImmunizationRepository
    {
        /// <summary>
        /// Gets patient's <see cref="PastImmunization"/> of the <see cref="PastImmunizationStatus"/>.
        /// If they exist, otherwise they don't preserve in the result.
        /// </summary>
        /// <param name="patientId">Id of the patient.</param>
        /// <param name="pastImmunizationStatuses">Arrays of the required statuses to consume.</param>
        /// <returns>Enumerable of <see cref="PastImmunization"/>.</returns>
        Task<IEnumerable<PastImmunization>> GetPastImmunizationsForPatientAsync(string patientId, PastImmunizationStatus[] pastImmunizationStatuses);
    }
}
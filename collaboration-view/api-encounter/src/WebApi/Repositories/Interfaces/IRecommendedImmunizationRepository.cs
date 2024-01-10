// <copyright file="IRecommendedImmunizationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="RecommendedImmunization"/> entities.
    /// </summary>
    internal interface IRecommendedImmunizationRepository
    {
        /// <summary>
        /// Gets patient's <see cref="RecommendedImmunization"/> of the <see cref="RecommendedImmunizationStatus"/>.
        /// If they exist, otherwise they don't preserve in the result.
        /// </summary>
        /// <param name="patientId">Id of the patient.</param>
        /// <param name="recommendedImmunizationStatuses">Arrays of the required statuses to consume.</param>
        /// <returns>Enumerable of <see cref="RecommendedImmunization"/>.</returns>
        Task<IEnumerable<RecommendedImmunization>> GetRecommendedImmunizationsForPatientAsync(string patientId, RecommendedImmunizationStatus[] recommendedImmunizationStatuses);
    }
}
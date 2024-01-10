// <copyright file="IAllergyIntoleranceRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="AllergyIntolerance"/> entities.
    /// </summary>
    internal interface IAllergyIntoleranceRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="AllergyIntolerance"/> that match the filtering parameters.
        /// </summary>
        /// <param name="patientId">Identifier of a specific patient.</param>
        /// <param name="status">Allergy intolerance status <see cref="AllergyIntoleranceClinicalStatus"/>.</param>
        /// <returns>Filtered items of <see cref="AllergyIntolerance"/>.</returns>
        Task<IEnumerable<AllergyIntolerance>> GetAllergyIntolerancesForPatientAsync(string patientId, AllergyIntoleranceClinicalStatus status);
    }
}

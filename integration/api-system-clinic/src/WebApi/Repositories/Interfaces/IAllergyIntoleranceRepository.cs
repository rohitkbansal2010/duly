// <copyright file="IAllergyIntoleranceRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="AllergyIntolerance"/> entities.
    /// </summary>
    public interface IAllergyIntoleranceRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="AllergyIntolerance"/> which match with the filter.
        /// </summary>
        /// <param name="patientId">Identifier of Patient.</param>
        /// <param name="clinicalStatus">Clinical status of allergy.</param>
        /// <returns>Filtered items of <see cref="AllergyIntolerance"/>.</returns>
        Task<IEnumerable<AllergyIntolerance>> GetConfirmedAllergyIntoleranceForPatientAsync(string patientId, ClinicalStatus clinicalStatus);
    }
}
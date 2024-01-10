// <copyright file="IConditionRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Condition"/> entities.
    /// </summary>
    public interface IConditionRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Condition"/> which match with the filter.
        /// </summary>
        /// <param name="patientId">Identifier of Patient.</param>
        /// <param name="status">Clinical status of condition.</param>
        /// <returns>Filtered items of <see cref="Condition"/>.</returns>
        Task<IEnumerable<Condition>> FindProblemsForPatientAsync(string patientId, ConditionClinicalStatus[] status);
    }
}
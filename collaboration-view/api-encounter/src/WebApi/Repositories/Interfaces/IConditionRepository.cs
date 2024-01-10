// <copyright file="IConditionRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.Condition"/> model.
    /// </summary>
    internal interface IConditionRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Models.Condition"/> that match the filtering parameters.
        /// </summary>
        /// <param name="patientId">Identifier of a specific patient.</param>
        /// <param name="conditionClinicalStatuses">Arrays of the required statuses <see cref="Models.ConditionClinicalStatus"/> to consume.</param>
        /// <returns>Filtered items of <see cref="Models.Condition"/>.</returns>
        Task<IEnumerable<Models.Condition>> GetConditionsForPatientAsync(string patientId, Models.ConditionClinicalStatus[] conditionClinicalStatuses);
    }
}

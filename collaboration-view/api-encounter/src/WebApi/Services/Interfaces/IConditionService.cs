// <copyright file="IConditionService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Contracts.HealthCondition"/> entity.
    /// </summary>
    public interface IConditionService
    {
        /// <summary>
        /// Retrieve <see cref="Contracts.HealthConditions"/> that represents information
        /// about health conditions (<see cref="Contracts.HealthCondition"/>)
        /// that the specific patient has/had, divided by the condition status.
        /// </summary>
        /// <param name="patientId">The identifier of a specific patient.</param>
        /// <returns>An instance of <see cref="Contracts.HealthConditions"/> for a specific patient.</returns>
        Task<Contracts.HealthConditions> GetHealthConditionsByPatientIdAsync(string patientId);
    }
}

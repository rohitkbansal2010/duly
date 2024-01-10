// <copyright file="IParticipantRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Participant"/> entities.
    /// </summary>
    public interface IParticipantRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Participant"/> which match with the filter.
        /// </summary>
        /// <param name="encounterId">Identifier of encounter.</param>
        /// <returns>Filtered items of <see cref="Participant"/>.</returns>
        Task<IEnumerable<Participant>> GetParticipantsByEncounterIdAsync(string encounterId);
    }
}

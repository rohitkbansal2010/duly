// <copyright file="IImmunizationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Immunization"/> entities.
    /// </summary>
    public interface IImmunizationRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Immunization"/> which match with the filter.
        /// </summary>
        /// <param name="patientId">Identifier of clinic.</param>
        /// <param name="includedDueStatuses">DueStatuses which should be included.</param>
        /// <returns>Filtered items of <see cref="Immunization"/>.</returns>
        Task<IEnumerable<Immunization>> GetImmunizationsForSpecificPatientAsync(string patientId, IEnumerable<DueStatus> includedDueStatuses);
    }
}
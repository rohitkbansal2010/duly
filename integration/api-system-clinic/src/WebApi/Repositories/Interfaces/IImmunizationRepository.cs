// <copyright file="IImmunizationRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Immunization"/> entities.
    /// </summary>
    public interface IImmunizationRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Immunization"/> which match with the filter.
        /// </summary>
        /// <param name="patientId">Identifier of Patient.</param>
        /// <param name="immunizationStatuses">Immunization statuses that should be included, if null or empty includes all.</param>
        /// <returns>Filtered items of <see cref="Immunization"/>.</returns>
        Task<IEnumerable<Immunization>> FindImmunizationsForPatientAsync(string patientId, ImmunizationStatus[] immunizationStatuses = null);
    }
}
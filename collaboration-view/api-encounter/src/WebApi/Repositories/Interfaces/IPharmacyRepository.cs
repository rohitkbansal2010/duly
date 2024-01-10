// -----------------------------------------------------------------------
// <copyright file="IPharmacyRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.Pharmacy"/> entities.
    /// </summary>
    internal interface IPharmacyRepository
    {
        /// <summary>
        /// Retrieve all items of <see cref="Models.Pharmacy"/> that match the filtering parameters.
        /// </summary>
        /// <param name="patientId">Identifier of a specific patient.</param>
        /// <returns>Filtered items of <see cref="Models.Pharmacy"/>.</returns>
        Task<Models.Pharmacy> GeTPreferredPharmacyByPatientIdAsync(string patientId);
    }
}

// <copyright file="IPharmacyRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Pharmacy"/> entities.
    /// </summary>
    public interface IPharmacyRepository
    {
        /// <summary>
        /// Returns Preferred  <see cref="Pharmacy"/> Detail by a Patient.
        /// </summary>
        /// <param name="patientId">Patient Id.</param>
        /// <returns>Details of Preferred Pharmacy by a Patient.</returns>
        Task<Pharmacy> GetPreferredPharmacyByPatientIdAsync(string patientId);
    }
}
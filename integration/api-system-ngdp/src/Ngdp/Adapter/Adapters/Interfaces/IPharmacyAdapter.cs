// <copyright file="IPharamacyAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Adapter.Adapters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// An adapter for working with "Pharmcy" in ngdp database via stored procedures that fetch details of Preferred Pharmacy.
    /// </summary>
    public interface IPharmacyAdapter
    { 
        /// <summary>
      /// Returns Preferred  <see cref="Pharmacy"/> Detail by a Patient.
      /// </summary>
      /// <param name="patientId">Patient Id.</param>
      /// <returns>Details of Preferred Pharmacy by a Patient.</returns>
        Task<Pharmacy> FindPreferredPharmacyByPatientIdAsync(string patientId);

    }
}
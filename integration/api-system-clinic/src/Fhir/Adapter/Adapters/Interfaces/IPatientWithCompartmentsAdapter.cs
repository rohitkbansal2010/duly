// <copyright file="IPatientWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// Adapter to work on Patient with compartments.
    /// </summary>
    public interface IPatientWithCompartmentsAdapter
    {
        /// <summary>
        /// Gets Patient and its compartments.
        /// </summary>
        /// <param name="patientId">Identifier of the patient.</param>
        /// <returns><see cref="PatientWithCompartments"/> item or null if necessary item is not found.</returns>
        Task<PatientWithCompartments> FindPatientByIdAsync(string patientId);

        /// <summary>
        /// Gets Patient and its compartments.
        /// </summary>
        /// <param name="identifiers">Identifiers of the patient.</param>
        /// <returns>All data that satisfies defined identifiers.</returns>
        Task<IEnumerable<PatientWithCompartments>> FindPatientsByIdentifiersAsync(string[] identifiers);

        /// <summary>
        /// Gets Patient Photo.
        /// </summary>
        /// <param name="request">Identifiers of the PatientPhotoByParam.</param>
        /// <returns>All data that satisfies defined identifiers.</returns>
        Task<GetPatientPhotoRoot> FindPatientPhotoByIdentifiersAsync(PatientPhotoByParam request);
    }
}
// <copyright file="IMedicationStatementWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// Adapter to work on Medication with compartments.
    /// </summary>
    public interface IMedicationStatementWithCompartmentsAdapter
    {
        /// <summary>
        /// Gets Medications and their compartments.
        /// </summary>
        /// <param name="criteria">Medication search criteria. Should correctly work only with patientId.</param>
        /// <returns>All data that satisfies search criteria.</returns>
        Task<IEnumerable<MedicationStatementWithCompartments>> FindMedicationsWithCompartmentsAsync(MedicationSearchCriteria criteria);

        /// <summary>
        /// Gets Medications and their compartments.
        /// </summary>
        /// <param name="criteria">Medication search criteria. Should correctly work only with patientId.</param>
        /// <returns>All data that satisfies search criteria.</returns>
        Task<IEnumerable<MedicationRequestWithCompartments>> FindMedicationsRequestWithCompartmentsAsync(MedicationSearchCriteria criteria);
    }
}

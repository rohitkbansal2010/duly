// <copyright file="MedicationRequestWithCompartments.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    /// <summary>
    /// Medication and its compartments.
    /// </summary>
    public class MedicationRequestWithCompartments : IResourceWithCompartments<R4.MedicationRequest>
    {
        /// <summary>
        /// Medication.
        /// </summary>
        public R4.MedicationRequest Resource { get; set; }

        /// <summary>
        /// Practitioners of the medication.
        /// </summary>
        public PractitionerWithRoles Practitioner { get; set; }
    }
}

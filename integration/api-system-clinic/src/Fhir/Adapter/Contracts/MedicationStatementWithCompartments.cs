// <copyright file="MedicationStatementWithCompartments.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias stu3;
using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    /// <summary>
    /// Medication and its compartments.
    /// </summary>
    public class MedicationStatementWithCompartments : IResourceWithCompartments<STU3.MedicationStatement>
    {
        /// <summary>
        /// Medication.
        /// </summary>
        public STU3.MedicationStatement Resource { get; set; }

        /// <summary>
        /// Practitioners of the medication.
        /// </summary>
        public PractitionerWithRolesSTU3 Practitioner { get; set; }
    }
}

// <copyright file="EncounterWithCompartments.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    /// <summary>
    /// Encounter and its compartments.
    /// </summary>
    public class EncounterWithCompartments : IResourceWithCompartments<R4.Encounter>
    {
        /// <summary>
        /// Encounter.
        /// </summary>
        public R4.Encounter Resource { get; set; }

        /// <summary>
        /// Encounter's subject.
        /// </summary>
        public R4.Patient Patient { get; set; }

        /// <summary>
        /// Historical information about patient visits.
        /// </summary>
        public bool DoesPatientHavePastVisits { get; set; }

        /// <summary>
        /// Practitioners in the encounter.
        /// </summary>
        public PractitionerWithRoles[] Practitioners { get; set; }
    }
}

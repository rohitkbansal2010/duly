// <copyright file="ObservationWithCompartments.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    /// <summary>
    /// Observation with compartments.
    /// </summary>
    public class ObservationWithCompartments
    {
        /// <summary>
        /// Observation.
        /// </summary>
        public R4.Observation Resource { get; set; }
    }
}
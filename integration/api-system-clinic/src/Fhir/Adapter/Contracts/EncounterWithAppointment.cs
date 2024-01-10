// <copyright file="EncounterWithAppointment.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias stu3;
using stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Contracts
{
    /// <summary>
    /// Encounter and its compartments.
    /// </summary>
    public class EncounterWithAppointment : IResourceWithCompartments<Encounter>
    {
        /// <summary>
        /// Encounter.
        /// </summary>
        public Encounter Resource { get; set; }

        /// <summary>
        /// Appointment resource. This element is populated if this encounter is from an appointment.
        /// </summary>
        public Appointment Appointment { get; set; }

        /// <summary>
        /// Practitioners in the encounter.
        /// </summary>
        public PractitionerWithRolesSTU3[] Practitioners { get; set; }
    }
}
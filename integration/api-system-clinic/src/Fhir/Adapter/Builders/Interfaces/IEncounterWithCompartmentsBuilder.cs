// <copyright file="IEncounterWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
extern alias stu3;

using Duly.Clinic.Fhir.Adapter.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;
using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Interfaces
{
    /// <summary>
    /// Can build EncounterWithCompartments.
    /// </summary>
    public interface IEncounterWithCompartmentsBuilder
    {
        /// <summary>
        /// Gets Encounters from Bundles.
        /// </summary>
        /// <param name="searchResult">Collection of search results.</param>
        /// <param name="date">Encounters date limitation.</param>
        /// <returns>Complete resources.</returns>
        Task<EncounterWithCompartments[]> ExtractEncountersWithCompartmentsAsync(IEnumerable<R4.Bundle.EntryComponent> searchResult, DateTime date);

        /// <summary>
        /// Gets Encounters with participants from Bundle.
        /// </summary>
        /// <param name="searchResult">Collection of search results.</param>
        /// <param name="shouldLeaveActivePractitioners">Condition to filter search practitioners by active status.</param>
        /// <returns>Complete resources.</returns>
        Task<EncounterWithParticipants> ExtractEncounterWithParticipantsAsync(IEnumerable<R4.Bundle.EntryComponent> searchResult, bool shouldLeaveActivePractitioners = false);

        /// <summary>
        /// Gets Encounters from Bundles.
        /// </summary>
        /// <param name="searchResult">Collection of search results.</param>
        /// <returns>Complete items <see cref="EncounterWithAppointment"/> from <see cref="searchResult"/>.</returns>
        Task<EncounterWithAppointment[]> ExtractEncountersWithAppointmentsAsync(IEnumerable<STU3.Bundle.EntryComponent> searchResult);
    }
}
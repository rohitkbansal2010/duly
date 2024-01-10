// <copyright file="IEncounterWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// Adapter to work on Encounter with compartments.
    /// </summary>
    public interface IEncounterWithCompartmentsAdapter
    {
        /// <summary>
        /// Gets Encounters and their compartments.
        /// </summary>
        /// <param name="criteria">Search criteria. Should correctly work only with siteId and with StartDateTime. </param>
        /// <returns>All data that satisfies search criteria.</returns>
        Task<IEnumerable<EncounterWithCompartments>> FindEncountersWithCompartmentsAsync(SearchCriteria criteria);

        /// <summary>
        /// Gets Encounter and its participants.
        /// </summary>
        /// <param name="encounterId">Identity of the encounter.</param>
        /// <returns>Encounter with participants.</returns>
        Task<EncounterWithParticipants> FindEncounterWithParticipantsAsync(string encounterId);

        /// <summary>
        /// Gets Encounter with Appointments and their participants.
        /// </summary>
        /// <param name="criteria">Identity of the encounter.</param>
        /// <returns>Encounter with participants.</returns>
        Task<IEnumerable<EncounterWithAppointment>> FindEncountersWithAppointmentsAsync(EncounterSearchCriteria criteria);
    }
}
// <copyright file="ICareTeamWithCompartmentsAdapter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using System.Threading.Tasks;

namespace Duly.Clinic.Fhir.Adapter.Adapters.Interfaces
{
    /// <summary>
    /// Adapter to work on CareTeam with compartments.
    /// </summary>
    public interface ICareTeamWithCompartmentsAdapter
    {
        /// <summary>
        /// Gets Care Teams and their participants.
        /// </summary>
        /// <param name="careTeamSearchCriteria">Search criteria for CareTeam.</param>
        /// <returns>CareTeam with participants.</returns>
        Task<CareTeamWithParticipants[]> FindCareTeamsWithParticipantsAsync(CareTeamSearchCriteria careTeamSearchCriteria);
    }
}
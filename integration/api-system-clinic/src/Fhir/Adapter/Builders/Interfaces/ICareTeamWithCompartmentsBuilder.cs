// <copyright file="ICareTeamWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Fhir.Adapter.Contracts;
using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Interfaces
{
    /// <summary>
    /// Can build CareTeamWithCompartmentsBuilder.
    /// </summary>
    public interface ICareTeamWithCompartmentsBuilder
    {
        /// <summary>
        /// Extract care teams with participants from Bundles.
        /// </summary>
        /// <param name="searchResult">Entries from search.</param>
        /// <param name="endOfParticipation">Filter of participants by end period.</param>
        /// <param name="categoryCoding">Category of CareTeams.</param>
        /// <param name="shouldLeaveActivePractitioners">Condition to filter search practitioners by active status.</param>
        /// <returns>Extracted care teams with participants.</returns>
        Task<CareTeamWithParticipants[]> ExtractCareTeamsWithParticipantsAsync(IEnumerable<R4.Bundle.EntryComponent> searchResult, DateTimeOffset endOfParticipation, Coding categoryCoding, bool shouldLeaveActivePractitioners = false);
    }
}

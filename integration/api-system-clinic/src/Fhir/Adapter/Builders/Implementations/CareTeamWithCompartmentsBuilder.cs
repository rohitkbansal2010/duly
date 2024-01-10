// <copyright file="CareTeamWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Implementations
{
    /// <summary>
    /// <inheritdoc cref="ICareTeamWithCompartmentsBuilder"/>
    /// </summary>
    internal class CareTeamWithCompartmentsBuilder : ICareTeamWithCompartmentsBuilder
    {
        private readonly IPractitionerWithRolesBuilder _practitionerWithRolesBuilder;

        public CareTeamWithCompartmentsBuilder(IPractitionerWithRolesBuilder practitionerWithRolesBuilder)
        {
            _practitionerWithRolesBuilder = practitionerWithRolesBuilder;
        }

        public async Task<CareTeamWithParticipants[]> ExtractCareTeamsWithParticipantsAsync(
            IEnumerable<R4.Bundle.EntryComponent> searchResult,
            DateTimeOffset endOfParticipation,
            Coding categoryCoding,
            bool shouldLeaveActivePractitioners = false)
        {
            //searchResult = careTeams + practitioners
            var searchResultArray = searchResult.ToArray();
            var careTeams = searchResultArray
                .Select(component => component.Resource).OfType<R4.CareTeam>()
                .Where(careTeam => careTeam.Category
                    .Any(concept => concept.Coding.Contains(categoryCoding, new CodingsComparer()))).ToArray();

            var filteredComponents = FilterEntryComponent(careTeams, searchResultArray, endOfParticipation);

            var practitionersWithRoles = await _practitionerWithRolesBuilder.RetrievePractitionerWithRolesAsync(filteredComponents, shouldLeaveActivePractitioners);

            return BuildFromSearchResults(careTeams, practitionersWithRoles).ToArray();
        }

        private static IEnumerable<R4.Bundle.EntryComponent> FilterEntryComponent(R4.CareTeam[] careTeam, R4.Bundle.EntryComponent[] searchResultArray, DateTimeOffset endOfParticipation)
        {
            var participants = careTeam.SelectMany(team => team.Participant).ToArray();

            foreach (var component in searchResultArray.Where(component => component.Resource.GetType() == typeof(R4.Practitioner)))
            {
                var reference = component.Resource.ToReference();

                var member = participants.First(
                    participantComponent => participantComponent.Member.Reference == reference);

                if (ValidateMember(endOfParticipation, member))
                {
                    yield return component;
                }
            }
        }

        private static bool ValidateMember(DateTimeOffset endOfParticipation, R4.CareTeam.ParticipantComponent member)
        {
            return member.Period == null
                   || member.Period.EndElement == null
                   || member.Period.EndElement.Value == null
                   || member.Period.EndElement.BuildDateTimeOffset() >= endOfParticipation;
        }

        private static IEnumerable<CareTeamWithParticipants> BuildFromSearchResults(IEnumerable<R4.CareTeam> careTeams, PractitionerWithRoles[] practitionersWithRoles)
        {
            foreach (var careTeam in careTeams)
            {
                var references = careTeam.Participant.Select(component => component.Member.Reference);

                var practitionersWithRolesForCareTeam = practitionersWithRoles
                    .Where(roles => references.Contains(roles.Resource.ToReference())).ToArray();

                yield return BuildFromSearchResults(careTeam, practitionersWithRolesForCareTeam);
            }
        }

        private static CareTeamWithParticipants BuildFromSearchResults(R4.CareTeam careTeam, PractitionerWithRoles[] practitionersWithRoles)
        {
            var result = new CareTeamWithParticipants
            {
                Resource = careTeam,
                Practitioners = practitionersWithRoles,
            };

            return result;
        }
    }
}
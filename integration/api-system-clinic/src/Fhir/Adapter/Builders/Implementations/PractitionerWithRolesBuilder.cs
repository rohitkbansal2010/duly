// <copyright file="PractitionerWithRolesBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
extern alias stu3;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using R4 = r4::Hl7.Fhir.Model;
using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Implementations
{
    internal class PractitionerWithRolesBuilder : IPractitionerWithRolesBuilder
    {
        private readonly IFhirClientR4 _clientR4;
        private readonly IFhirClientSTU3 _clientStu3;
        private readonly ILogger<PractitionerWithRolesBuilder> _logger;

        public PractitionerWithRolesBuilder(IFhirClientR4 clientR4, IFhirClientSTU3 clientStu3, ILogger<PractitionerWithRolesBuilder> logger)
        {
            _clientR4 = clientR4;
            _clientStu3 = clientStu3;
            _logger = logger;
        }

        public PractitionerWithRoles[] ExtractPractitionerWithRoles(IEnumerable<R4.Bundle.EntryComponent> searchResult, bool shouldLeaveActivePractitioners = false)
        {
            var searchResultArray = searchResult.ToArray();
            var practitionerRoles = searchResultArray.Select(component => component.Resource).OfType<R4.PractitionerRole>().ToArray();
            var practitioners = GetFilteredPractitioners(searchResultArray, shouldLeaveActivePractitioners);
            var resourcesWithCompartments = BuildResourcesWithCompartments(practitioners, practitionerRoles);
            return resourcesWithCompartments.ToArray();
        }

        public async Task<PractitionerWithRoles[]> RetrievePractitionerWithRolesAsync(IEnumerable<R4.Bundle.EntryComponent> searchResults, bool shouldLeaveActivePractitioners = false)
        {
            var entryComponents = searchResults.Where(component => component.Resource is R4.Practitioner).ToArray();
            var practitioners = GetFilteredPractitioners(entryComponents, shouldLeaveActivePractitioners);
            var practitionerRoles = await FindPractitionerRoles(practitioners);
            var practitionersWithRoles = ExtractPractitionerWithRoles(practitionerRoles.Union(entryComponents), shouldLeaveActivePractitioners);
            return practitionersWithRoles;
        }

        public async Task<PractitionerWithRoles[]> RetrievePractitionerWithRolesSafeAsync(IEnumerable<R4.Bundle.EntryComponent> searchResults, bool shouldLeaveActivePractitioners = false)
        {
            var entryComponents = searchResults.Where(component => component.Resource is R4.Practitioner).ToArray();
            var practitioners = GetFilteredPractitioners(entryComponents, shouldLeaveActivePractitioners);
            var roles = await TryFindPractitioners(practitioners, entryComponents);
            var practitionersWithRoles = ExtractPractitionerWithRoles(roles, shouldLeaveActivePractitioners);
            return practitionersWithRoles;
        }

        public async Task<PractitionerWithRolesSTU3[]> RetrievePractitionerWithRolesAsync(IEnumerable<STU3.Bundle.EntryComponent> searchResults)
        {
            var entryComponents = searchResults.Where(component => component.Resource is STU3.Practitioner).ToArray();
            var practitioners = entryComponents.Select(component => component.Resource).OfType<STU3.Practitioner>();
            var practitionerRoles = await FindPractitionerRoles(practitioners);
            var practitionersWithRoles = ExtractPractitionerWithRoles(practitionerRoles.Union(entryComponents));
            return practitionersWithRoles;
        }

        private static PractitionerWithRolesSTU3[] ExtractPractitionerWithRoles(IEnumerable<STU3.Bundle.EntryComponent> searchResult)
        {
            var searchResultArray = searchResult.ToArray();
            var practitionerRoles = searchResultArray.Select(component => component.Resource).OfType<STU3.PractitionerRole>().ToArray();
            var practitioners = searchResultArray.Select(component => component.Resource).OfType<STU3.Practitioner>();
            var resourcesWithCompartments = BuildResourcesWithCompartments(practitioners, practitionerRoles);
            return resourcesWithCompartments.ToArray();
        }

        private static IEnumerable<R4.Practitioner> GetFilteredPractitioners(IEnumerable<R4.Bundle.EntryComponent> searchResultArray, bool shouldLeaveActivePractitioners)
        {
            var practitioners = searchResultArray.Select(component => component.Resource).OfType<R4.Practitioner>();
            return shouldLeaveActivePractitioners ? practitioners.Where(x => x.Active ?? false) : practitioners;
        }

        private static IEnumerable<PractitionerWithRoles> BuildResourcesWithCompartments(
            IEnumerable<R4.Practitioner> practitioners,
            R4.PractitionerRole[] practitionerRoles)
        {
            return practitioners.Select(practitioner => new PractitionerWithRoles
            {
                Resource = practitioner,
                Roles = FindRoles(practitioner, practitionerRoles)
            });
        }

        private static IEnumerable<PractitionerWithRolesSTU3> BuildResourcesWithCompartments(
            IEnumerable<STU3.Practitioner> practitioners,
            STU3.PractitionerRole[] practitionerRoles)
        {
            return practitioners.Select(practitioner => new PractitionerWithRolesSTU3
            {
                Resource = practitioner,
                Roles = FindRoles(practitioner, practitionerRoles)
            });
        }

        private static R4.PractitionerRole[] FindRoles(R4.Practitioner practitioner, R4.PractitionerRole[] practitionerRoles)
        {
            var practitionerReference = practitioner.ToReference();
            return practitionerRoles.Where(x => x.Practitioner.Reference.EndsWith(practitionerReference)).ToArray();
        }

        private static STU3.PractitionerRole[] FindRoles(STU3.Practitioner practitioner, STU3.PractitionerRole[] practitionerRoles)
        {
            var practitionerReference = practitioner.ToReference();
            return practitionerRoles.Where(x => x.Practitioner.Reference.EndsWith(practitionerReference)).ToArray();
        }

        private static SearchParams BuildSearchParamsForPractitioners(IEnumerable<Resource> practitioners)
        {
            var value = string.Join(SearchParamsExtensions.EntityKeysSeparator, practitioners.Select(practitioner => practitioner.ToReference()));
            return string.IsNullOrEmpty(value) ? null : new SearchParams().ByPractitioner(value);
        }

        private async Task<IEnumerable<R4.Bundle.EntryComponent>> TryFindPractitioners(IEnumerable<R4.Practitioner> practitioners, IEnumerable<R4.Bundle.EntryComponent> roles)
        {
            try
            {
                var practitionerRoles = await FindPractitionerRoles(practitioners);
                return roles.Union(practitionerRoles);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Failed to FindPractitionerRoles");
            }

            return roles;
        }

        private async Task<R4.Bundle.EntryComponent[]> FindPractitionerRoles(IEnumerable<R4.Practitioner> practitioners)
        {
            var searchParams = BuildSearchParamsForPractitioners(practitioners);
            if (searchParams == null)
                return Array.Empty<R4.Bundle.EntryComponent>();

            return await _clientR4.FindPractitionerRolesAsync(searchParams);
        }

        private async Task<STU3.Bundle.EntryComponent[]> FindPractitionerRoles(IEnumerable<STU3.Practitioner> practitioners)
        {
            var searchParams = BuildSearchParamsForPractitioners(practitioners);
            if (searchParams == null)
                return Array.Empty<STU3.Bundle.EntryComponent>();

            return await _clientStu3.FindPractitionerRolesAsync(searchParams);
        }
    }
}

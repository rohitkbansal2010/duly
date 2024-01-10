// <copyright file="MedicationRequestWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IMedicationStatementWithCompartmentsBuilder"/>
    /// </summary>
    internal class MedicationRequestWithCompartmentsBuilder : IMedicationRequestWithCompartmentsBuilder
    {
        public MedicationRequestWithCompartments[] ExtractMedicationRequestWithCompartments(
            IEnumerable<R4.Bundle.EntryComponent> searchResult,
            PractitionerWithRoles[] practitionersWithRoles)
        {
            var searchResultArray = searchResult.ToArray();

            var medicationStatements =
                searchResultArray.Select(x => x.Resource).OfType<R4.MedicationRequest>().ToArray();

            return medicationStatements
                .Select(statement => BuildFromSearchResults(statement, practitionersWithRoles))
                .ToArray();
        }

        private static MedicationRequestWithCompartments BuildFromSearchResults(
            R4.MedicationRequest statement,
            IEnumerable<PractitionerWithRoles> practitionersWithRoles)
        {
            var result = new MedicationRequestWithCompartments
            {
                Resource = statement,
                Practitioner = FindPractitionersForMedicationStatement(practitionersWithRoles, statement.Requester?.Reference),
            };

            return result;
        }

        private static PractitionerWithRoles FindPractitionersForMedicationStatement(
            IEnumerable<PractitionerWithRoles> practitionersWithRoles,
            string medicationStatementResourceRef)
        {
            return string.IsNullOrEmpty(medicationStatementResourceRef) ?
                null :
                practitionersWithRoles.FirstOrDefault(x => medicationStatementResourceRef.EndsWith(x.Resource.ToReference()));
        }
    }
}

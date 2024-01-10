// <copyright file="MedicationStatementWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias stu3;

using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using System.Collections.Generic;
using System.Linq;

using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Implementations
{
    /// <summary>
    /// <inheritdoc cref="IMedicationStatementWithCompartmentsBuilder"/>
    /// </summary>
    internal class MedicationStatementWithCompartmentsBuilder : IMedicationStatementWithCompartmentsBuilder
    {
        public MedicationStatementWithCompartments[] ExtractMedicationWithCompartments(
            IEnumerable<STU3.Bundle.EntryComponent> searchResult,
            PractitionerWithRolesSTU3[] practitionersWithRoles)
        {
            var searchResultArray = searchResult.ToArray();

            var medicationStatements =
                searchResultArray.Select(x => x.Resource).OfType<STU3.MedicationStatement>().ToArray();

            return medicationStatements
                .Select(statement => BuildFromSearchResults(statement, practitionersWithRoles))
                .ToArray();
        }

        private static MedicationStatementWithCompartments BuildFromSearchResults(
            STU3.MedicationStatement statement,
            IEnumerable<PractitionerWithRolesSTU3> practitionersWithRoles)
        {
            var result = new MedicationStatementWithCompartments
            {
                Resource = statement,
                Practitioner = FindPractitionersForMedicationStatement(practitionersWithRoles, statement.InformationSource?.Reference),
            };

            return result;
        }

        private static PractitionerWithRolesSTU3 FindPractitionersForMedicationStatement(
            IEnumerable<PractitionerWithRolesSTU3> practitionersWithRoles,
            string medicationStatementResourceRef)
        {
            return string.IsNullOrEmpty(medicationStatementResourceRef) ?
                null :
                practitionersWithRoles.FirstOrDefault(x => medicationStatementResourceRef.EndsWith(x.Resource.ToReference()));
        }
    }
}

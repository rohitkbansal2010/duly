// <copyright file="IPractitionerWithRolesBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias stu3;
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;
using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Interfaces
{
    /// <summary>
    /// Can build PractitionerWithRoles.
    /// </summary>
    public interface IPractitionerWithRolesBuilder
    {
        /// <summary>
        /// Extract practitioners with roles from Bundles.
        /// </summary>
        /// <param name="searchResult">Entries from search.</param>
        /// <param name="shouldLeaveActivePractitioners">Condition to filter search result by active status.</param>
        /// <returns>Extracted practitioners.</returns>
        PractitionerWithRoles[] ExtractPractitionerWithRoles(IEnumerable<R4.Bundle.EntryComponent> searchResult, bool shouldLeaveActivePractitioners = false);

        /// <summary>
        /// Find Practitioners in <see cref="searchResults"/> and fetch roles for them.
        /// </summary>
        /// <param name="searchResults">Entries from search.</param>
        /// <param name="shouldLeaveActivePractitioners">Condition to filter search result by active status.</param>
        /// <returns>Extracted practitioners.</returns>
        Task<PractitionerWithRoles[]> RetrievePractitionerWithRolesAsync(IEnumerable<R4.Bundle.EntryComponent> searchResults, bool shouldLeaveActivePractitioners = false);

        /// <summary>
        /// Find Practitioners in <see cref="searchResults"/> and fetch roles for them. Doesn't throw if roles retrieval fail.
        /// </summary>
        /// <param name="searchResults">Entries from search.</param>
        /// <param name="shouldLeaveActivePractitioners">Condition to filter search result by active status.</param>
        /// <returns>Extracted practitioners.</returns>
        Task<PractitionerWithRoles[]> RetrievePractitionerWithRolesSafeAsync(IEnumerable<R4.Bundle.EntryComponent> searchResults, bool shouldLeaveActivePractitioners = false);

        /// <summary>
        /// Find Practitioners in <see cref="searchResults"/> and fetch roles for them.
        /// </summary>
        /// <param name="searchResults">Entries from search.</param>
        /// <returns>Extracted practitioners.</returns>
        Task<PractitionerWithRolesSTU3[]> RetrievePractitionerWithRolesAsync(IEnumerable<STU3.Bundle.EntryComponent> searchResults);
    }
}

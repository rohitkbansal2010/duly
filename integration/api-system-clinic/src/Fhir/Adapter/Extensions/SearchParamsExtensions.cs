// <copyright file="SearchParamsExtensions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Support;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Duly.Clinic.Fhir.Adapter.Extensions
{
    public static class SearchParamsExtensions
    {
        public const char EntityKeysSeparator = ',';
        private const string DateKey = "date";

        /// <summary>
        /// Adds Where clause by Patient reference.
        /// </summary>
        /// <param name="searchParams">Search parameters chain.</param>
        /// <param name="patientId">Patient reference.</param>
        /// <returns>Augmented search params.</returns>
        public static SearchParams ByPatientId(this SearchParams searchParams, string patientId)
        {
            return searchParams.ByPatientReference($"Patient/{patientId}");
        }

        /// <summary>
        /// Adds Where clause by Patient reference.
        /// </summary>
        /// <param name="searchParams">Search parameters chain.</param>
        /// <param name="patientReference">Patient reference.</param>
        /// <returns>Augmented search params.</returns>
        public static SearchParams ByPatientReference(this SearchParams searchParams, string patientReference)
        {
            return searchParams.Add("patient", patientReference);
        }

        public static SearchParams BySubjectReferences(this SearchParams searchParams, IEnumerable<Resource> resources)
        {
            var subjectQuery = string.Join(SearchParamsExtensions.EntityKeysSeparator, resources.Select(resource => resource.ToReference()));
            return searchParams.Add("subject", subjectQuery);
        }

        public static SearchParams BySiteId(this SearchParams searchParams, string siteId)
        {
            return searchParams.Add("location", siteId);
        }

        public static SearchParams ByEqualDate(this SearchParams searchParams, DateTime date)
        {
            return searchParams.Add(DateKey, date.ToFhirDateTime(TimeSpan.Zero));
        }

        public static SearchParams ByLessDate(this SearchParams searchParams, DateTime date)
        {
            return searchParams.Add(DateKey, $"lt{date.ToFhirDateTime(TimeSpan.Zero)}");
        }

        public static SearchParams ByLessOrEqualDate(this SearchParams searchParams, DateTime date)
        {
            return searchParams.Add(DateKey, $"le{date.ToFhirDateTime(TimeSpan.Zero)}");
        }

        public static SearchParams ByGreaterOrEqualDate(this SearchParams searchParams, DateTime date)
        {
            return searchParams.Add(DateKey, $"ge{date.ToFhirDateTime(TimeSpan.Zero)}");
        }

        public static SearchParams ById(this SearchParams searchParams, string id)
        {
            return searchParams.Add("_id", id);
        }

        public static SearchParams ByIdentifiers(this SearchParams searchParams, string[] identifiers)
        {
            return searchParams.Add("identifier", string.Join(EntityKeysSeparator, identifiers));
        }

        public static SearchParams ByStatus(this SearchParams searchParams, string status)
        {
            return searchParams.Add("status", status);
        }

        public static SearchParams ByClinicalStatus(this SearchParams searchParams, string clinicalStatus)
        {
            return searchParams.Add("clinical-status", clinicalStatus);
        }

        public static SearchParams ByCodes(this SearchParams searchParams, params string[] codes)
        {
            var codesValue = string.Join(EntityKeysSeparator, codes);
            return searchParams.Add("code", codesValue);
        }

        public static SearchParams ByCategory(this SearchParams searchParams, string category)
        {
            return searchParams.Add("category", category);
        }

        public static SearchParams ByPractitioner(this SearchParams searchParams, string practitioner)
        {
            return searchParams.Add("practitioner", practitioner);
        }

        public static SearchParams AddIncludes(this SearchParams searchParams, string[] listOfIncludes)
        {
            foreach (var include in listOfIncludes)
            {
                searchParams.Include(include);
            }

            return searchParams;
        }

        private static void Include(
            this SearchParams qry,
            string path,
            IncludeModifier modifier = IncludeModifier.None)
        {
            qry.Include.Add((path, modifier));
        }
    }
}
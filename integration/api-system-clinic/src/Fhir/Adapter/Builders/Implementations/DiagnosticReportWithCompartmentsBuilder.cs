// <copyright file="DiagnosticReportWithCompartmentsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions.Fhir;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Fhir.Adapter.Builders.Implementations
{
    internal class DiagnosticReportWithCompartmentsBuilder : IDiagnosticReportWithCompartmentsBuilder
    {
        private readonly IPractitionerWithRolesBuilder _practitionerWithRolesBuilder;

        public DiagnosticReportWithCompartmentsBuilder(IPractitionerWithRolesBuilder practitionerWithRolesBuilder)
        {
            _practitionerWithRolesBuilder = practitionerWithRolesBuilder;
        }

        public async Task<IEnumerable<DiagnosticReportWithCompartments>> ExtractDiagnosticReportWithCompartmentsAsync(IEnumerable<R4.Bundle.EntryComponent> searchResult)
        {
            var searchResultArray = searchResult.ToArray();
            if (searchResultArray.Length == 0)
            {
                return Array.Empty<DiagnosticReportWithCompartments>();
            }

            var diagnosticReports = searchResultArray.Select(component => component.Resource).OfType<R4.DiagnosticReport>();
            var observations = searchResultArray
                .Select(component => component.Resource)
                .OfType<R4.Observation>()
                .ToDictionary(observation => observation.ToReference());

            var practitionersWithRoles = await _practitionerWithRolesBuilder.RetrievePractitionerWithRolesAsync(searchResultArray);

            var practitionerWithRolesMap = practitionersWithRoles
                .ToDictionary(practitioner => practitioner.Resource.ToReference());

            return diagnosticReports.Select(report => CreateDiagnosticReport(report, observations, practitionerWithRolesMap));
        }

        public async Task<DiagnosticReportWithCompartments> ExtractDiagnosticReportWithCompartmentsByIdAsync(IEnumerable<R4.Bundle.EntryComponent> searchResult)
        {
            var searchResultArray = searchResult.ToArray();

            var diagnosticReport = searchResultArray.Select(component => component.Resource).OfType<R4.DiagnosticReport>().SingleOrDefault();
            if (diagnosticReport == null)
            {
                return null;
            }

            var observations = searchResultArray
                .Select(component => component.Resource)
                .OfType<R4.Observation>()
                .ToDictionary(observation => observation.ToReference());
            var practitionersWithRoles = await _practitionerWithRolesBuilder.RetrievePractitionerWithRolesAsync(searchResultArray);
            var practitionerWithRolesMap = practitionersWithRoles
                .ToDictionary(practitioner => practitioner.Resource.ToReference());
            return CreateDiagnosticReport(diagnosticReport, observations, practitionerWithRolesMap);
        }

        private static DiagnosticReportWithCompartments CreateDiagnosticReport(
            R4.DiagnosticReport report,
            IReadOnlyDictionary<string, R4.Observation> observations,
            IReadOnlyDictionary<string, PractitionerWithRoles> practitionersWithRoles)
        {
            var observationsMatched = FindObservations(report, observations);
            var practitionerWithRolesMatched = FindPractitionerWithRoles(report, practitionersWithRoles);

            return new DiagnosticReportWithCompartments
            {
                Resource = report,
                Observations = observationsMatched.ToArray(),
                Performers = practitionerWithRolesMatched.ToArray()
            };
        }

        private static IEnumerable<PractitionerWithRoles> FindPractitionerWithRoles(R4.DiagnosticReport report, IReadOnlyDictionary<string, PractitionerWithRoles> practitionersWithRoles)
        {
            foreach (var practitionerRef in report.Performer.Select(x => x.Reference))
            {
                if (practitionersWithRoles.TryGetValue(practitionerRef, out var practitionerWithRoles))
                    yield return practitionerWithRoles;
            }
        }

        private static IEnumerable<R4.Observation> FindObservations(R4.DiagnosticReport report, IReadOnlyDictionary<string, R4.Observation> observations)
        {
            foreach (var observationRef in report.Result.Select(x => x.Reference))
            {
                if (observations.TryGetValue(observationRef, out var observation))
                    yield return observation;
            }
        }
    }
}
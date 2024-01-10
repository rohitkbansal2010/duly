// <copyright file="DiagnosticReportSearchCriteriaTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Fhir.Adapter.Tests.Contracts
{
    [TestFixture]
    public class DiagnosticReportSearchCriteriaTests
    {
        public static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(
                new DiagnosticReportSearchCriteria
                {
                    PatientId = "id",
                    StartDateTime = new DateTime(2020, 11, 11),
                    EndDateTime = new DateTime(2021, 11, 11),
                    Codes = new[] { "code1", "code2" },
                });
            yield return new TestCaseData(
                new DiagnosticReportSearchCriteria
                {
                    PatientId = "id",
                });
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ToSearchParamsTest_ShouldSucceed(DiagnosticReportSearchCriteria criteria)
        {
            criteria.Invoking(x => x.ToSearchParams()).Should().NotThrow();
        }

        [Test]
        public void ToSearchParamsTest_ShouldThrowContradictoryQuery()
        {
            var criteria = new DiagnosticReportSearchCriteria
            {
                PatientId = "id",
                StartDateTime = new DateTime(2020, 11, 11),
                EndDateTime = new DateTime(2019, 11, 11)
            };

            criteria.Invoking(x => x.ToSearchParams()).Should().Throw<ContradictoryQueryException>()
                .WithMessage("Diagnostic report query EndDateTime < StartDateTime");
        }
    }
}

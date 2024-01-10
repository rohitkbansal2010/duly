// <copyright file="ObservationSearchCriteriaTests.cs" company="Duly Health and Care">
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
    public class ObservationSearchCriteriaTests
    {
        public static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(
                new ObservationSearchCriteria()
                {
                    PatientId = "id",
                    Category = "test",
                    StartDateTime = new DateTime(2020, 11, 11),
                    EndDateTime = new DateTime(2021, 11, 11)
                });
            yield return new TestCaseData(
                new ObservationSearchCriteria()
                {
                    PatientId = "id",
                    Category = "test",
                    Codes = new[] { "code1", "code2" },
                    StartDateTime = new DateTime(2020, 11, 11),
                    EndDateTime = new DateTime(2021, 11, 11)
                });
            yield return new TestCaseData(
                new ObservationSearchCriteria()
                {
                    PatientId = "id",
                    Category = "test",
                    StartDateTime = new DateTime(2020, 11, 11),
                    EndDateTime = new DateTime(2021, 11, 11)
                });
            yield return new TestCaseData(
                new ObservationSearchCriteria()
                {
                    PatientId = "id",
                    Category = "test",
                });
            yield return new TestCaseData(
                new ObservationSearchCriteria()
                {
                    PatientId = "id",
                    Category = "test",
                    StartDateTime = new DateTime(2020, 11, 11),
                });
            yield return new TestCaseData(
                new ObservationSearchCriteria()
                {
                    PatientId = "id",
                    Category = "test",
                    EndDateTime = new DateTime(2020, 11, 11),
                });
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ToSearchParamsTest_ShouldSucceed(ObservationSearchCriteria criteria)
        {
            criteria.Invoking(x => x.ToSearchParams()).Should().NotThrow();
        }

        [Test]
        public void ToSearchParamsTest_ShouldThrowParameterMissing()
        {
            var criteria = new ObservationSearchCriteria() { PatientId = "id" };

            criteria.Invoking(x => x.ToSearchParams()).Should().Throw<MandatoryQueryParameterMissingException>()
                .WithMessage("Observation Category is missing.");
        }

        [Test]
        public void ToSearchParamsTest_ShouldThrowContradictoryQuery()
        {
            var criteria = new ObservationSearchCriteria()
            {
                PatientId = "id",
                Category = "test",
                StartDateTime = new DateTime(2020, 11, 11),
                EndDateTime = new DateTime(2019, 11, 11)
            };

            criteria.Invoking(x => x.ToSearchParams()).Should().Throw<ContradictoryQueryException>()
                .WithMessage("Observation query EndDateTime < StartDateTime");
        }
    }
}
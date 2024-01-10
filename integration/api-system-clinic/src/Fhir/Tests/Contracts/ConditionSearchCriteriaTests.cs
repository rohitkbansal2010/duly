// <copyright file="ConditionSearchCriteriaTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Exceptions;
using FluentAssertions;
using Hl7.Fhir.Utility;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Contracts
{
    [TestFixture]
    public class ConditionSearchCriteriaTests
    {
        public static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(
                new ConditionSearchCriteria()
                {
                    PatientId = "id",
                    Categories = new[] { "test" },
                    Statuses = new[]
                    {
                        R4.Condition.ConditionClinicalStatusCodes.Active,
                        R4.Condition.ConditionClinicalStatusCodes.Inactive,
                        R4.Condition.ConditionClinicalStatusCodes.Resolved
                    }
                });
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ToSearchParamsTest_ShouldSucceed(ConditionSearchCriteria criteria)
        {
            criteria.Invoking(x => x.ToSearchParams()).Should().NotThrow();
            var result = criteria.ToSearchParams();
            result.Should().BeOfType<Hl7.Fhir.Rest.SearchParams>();
            result.Parameters.Any(x => x.Item1 == "clinical-status"
                                       && x.Item2.Contains(
                                           R4.Condition.ConditionClinicalStatusCodes.Active.GetLiteral())
                                       && x.Item2.Contains(
                                           R4.Condition.ConditionClinicalStatusCodes.Inactive.GetLiteral())
                                       && x.Item2.Contains(
                                           R4.Condition.ConditionClinicalStatusCodes.Resolved.GetLiteral()))
                .Should().BeTrue();
        }

        [Test]
        public void ToSearchParamsTest_ShouldThrowParameterMissing()
        {
            var criteria = new ConditionSearchCriteria { PatientId = "id" };

            criteria.Invoking(x => x.ToSearchParams()).Should().Throw<MandatoryQueryParameterMissingException>()
                .WithMessage("Condition Category is missing.");
        }

        [Test]
        public void ToSearchParamsTest_ShouldThrowParameterIdMissing()
        {
            var criteria = new ConditionSearchCriteria();

            criteria.Invoking(x => x.ToSearchParams()).Should().Throw<MandatoryQueryParameterMissingException>()
                .WithMessage("Patient Id is missing.");
        }

        [Test]
        public void ToSearchParamsTest_ShouldThrowContradictoryQuery()
        {
            var criteria = new ConditionSearchCriteria()
            {
                PatientId = "id",
                Categories = new[] { "test", "test2" },
            };

            criteria.Invoking(x => x.ToSearchParams()).Should().Throw<ContradictoryQueryException>()
                .WithMessage("Current version of EPIC supports only one category at a time.");
        }
    }
}
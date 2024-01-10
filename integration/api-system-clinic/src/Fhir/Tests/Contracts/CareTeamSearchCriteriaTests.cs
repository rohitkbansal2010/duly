// <copyright file="CareTeamSearchCriteriaTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Exceptions;
using FluentAssertions;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System;

namespace Fhir.Adapter.Tests.Contracts
{
    [TestFixture]
    public class CareTeamSearchCriteriaTests
    {
        [Test]
        public void ToSearchParamsTest_PatientReference_ShouldThrowParameterMissing()
        {
            var criteria = new CareTeamSearchCriteria
            {
                Status = "aa",
                CategoryCoding = new Coding(),
                EndOfParticipation = DateTimeOffset.UnixEpoch
            };

            criteria.Invoking(x => x.ToSearchParams()).Should().Throw<MandatoryQueryParameterMissingException>()
                .WithMessage("PatientReference is missing.");
        }

        [Test]
        public void ToSearchParamsTest_Status_ShouldThrowParameterMissing()
        {
            var criteria = new CareTeamSearchCriteria
            {
                PatientReference = "id",
                CategoryCoding = new Coding(),
                EndOfParticipation = DateTimeOffset.UnixEpoch
            };

            criteria.Invoking(x => x.ToSearchParams()).Should().Throw<MandatoryQueryParameterMissingException>()
                .WithMessage("Status is missing.");
        }

        [Test]
        public void ToSearchParamsTest_CategoryCoding_ShouldThrowParameterMissing()
        {
            var criteria = new CareTeamSearchCriteria
            {
                PatientReference = "id",
                Status = "aa",
                EndOfParticipation = DateTimeOffset.UnixEpoch
            };

            criteria.Invoking(x => x.ToSearchParams()).Should().Throw<MandatoryQueryParameterMissingException>()
                .WithMessage("Coding is missing.");
        }

        [Test]
        public void ToSearchParamsTest_EndOfParticipation_ShouldThrowParameterMissing()
        {
            var criteria = new CareTeamSearchCriteria
            {
                PatientReference = "id",
                Status = "aa",
                CategoryCoding = new Coding(),
            };

            criteria.Invoking(x => x.ToSearchParams()).Should().Throw<MandatoryQueryParameterMissingException>()
                .WithMessage("EndOfParticipation is missing.");
        }
    }
}

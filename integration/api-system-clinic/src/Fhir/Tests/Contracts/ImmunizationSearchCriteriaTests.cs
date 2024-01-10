// <copyright file="ImmunizationSearchCriteriaTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Fhir.Adapter.Tests.Contracts
{
    [TestFixture]
    public class ImmunizationSearchCriteriaTests
    {
        [Test]
        public void ToSearchParamsTest_HasNoParameters_ShouldThrowException_Test()
        {
            // Arrange
            var criteria = new ImmunizationSearchCriteria();

            // Act and Assert
            criteria.Invoking(c => c.ToSearchParams()).Should().Throw<MandatoryQueryParameterMissingException>();
        }

        [Test]
        public void ToSearchParamsTest_HasPatientID_ShouldSucceed()
        {
            // Arrange
            var criteria = new ImmunizationSearchCriteria
            {
                PatientId = "patient_id"
            };

            // Act
            var result = criteria.ToSearchParams();

            // Assert
            result.Parameters.Should().HaveCount(1);
            result.Parameters.Should().Contain(p => p.Item1 == "patient" && p.Item2 == $"Patient/{criteria.PatientId}");
        }

        [Test]
        public void ToSearchParamsTest_HasEmptyStatuses_ShouldSucceed()
        {
            // Arrange
            var criteria = new ImmunizationSearchCriteria
            {
                PatientId = "patientId",
                Statuses = Array.Empty<string>()
            };

            // Act
            var result = criteria.ToSearchParams();

            // Assert
            result.Parameters.Should().HaveCount(1);
            result.Parameters.Should().NotContain(p => p.Item1 == "status");
        }

        [Test]
        public void ToSearchParamsTest_HasNonEmptyStatuses_ShouldSucceed()
        {
            // Arrange
            var criteria = new ImmunizationSearchCriteria
            {
                PatientId = "patientId",
                Statuses = new[]
                {
                    "status1"
                }
            };

            // Act
            var result = criteria.ToSearchParams();

            // Assert
            result.Parameters.Should().HaveCount(2);
            result.Parameters.Should().Contain(p => p.Item1 == "status" && p.Item2 == criteria.Statuses.First());
        }

        [Test]
        public void ToSearchParamsTest_HasMoreThanOneStatus_ShouldSucceed()
        {
            // Arrange
            var criteria = new ImmunizationSearchCriteria
            {
                PatientId = "patientId",
                Statuses = new[]
                {
                    "status1",
                    "status2"
                }
            };

            // Act
            var result = criteria.ToSearchParams();

            // Assert
            result.Parameters.Should().HaveCount(2);
            result.Parameters.Should().Contain(p => p.Item1 == "status" && p.Item2 == "status1,status2");
        }
    }
}
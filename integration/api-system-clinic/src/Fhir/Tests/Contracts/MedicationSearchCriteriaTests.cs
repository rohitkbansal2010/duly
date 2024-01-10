// <copyright file="MedicationSearchCriteriaTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Fhir.Adapter.Tests.Contracts
{
    [TestFixture]
    public class MedicationSearchCriteriaTests
    {
        [Test]
        public void ToSearchParamsTest_HasNoParameters_ShouldSucceed()
        {
            // Arrange
            var criteria = new MedicationSearchCriteria
            {
                PatientId = null,
                Categories = null,
                Status = null
            };

            // Act
            var result = criteria.ToSearchParams();

            // Assert
            result.Parameters.Should().BeEmpty();
        }

        [Test]
        public void ToSearchParamsTest_HasPatientID_ShouldSucceed()
        {
            // Arrange
            var criteria = new MedicationSearchCriteria
            {
                PatientId = "patient_id",
                Categories = null,
                Status = null
            };

            // Act
            var result = criteria.ToSearchParams();

            // Assert
            result.Parameters.Should().HaveCount(1);
            result.Parameters.Should().Contain(p => p.Item1 == "patient" && p.Item2 == $"Patient/{criteria.PatientId}");
        }

        [Test]
        public void ToSearchParamsTest_HasStatus_ShouldSucceed()
        {
            // Arrange
            var criteria = new MedicationSearchCriteria
            {
                PatientId = null,
                Categories = null,
                Status = "status_value"
            };

            // Act
            var result = criteria.ToSearchParams();

            // Assert
            result.Parameters.Should().HaveCount(1);
            result.Parameters.Should().Contain(p => p.Item1 == "status" && p.Item2 == criteria.Status);
        }

        [Test]
        public void ToSearchParamsTest_HasEmptyCategories_ShouldSucceed()
        {
            // Arrange
            var criteria = new MedicationSearchCriteria
            {
                PatientId = null,
                Categories = Array.Empty<string>(),
                Status = null
            };

            // Act
            var result = criteria.ToSearchParams();

            // Assert
            result.Parameters.Should().BeEmpty();
        }

        [Test]
        public void ToSearchParamsTest_HasNonEmptyCategories_ShouldSucceed()
        {
            // Arrange
            var criteria = new MedicationSearchCriteria
            {
                PatientId = null,
                Categories = new[] { "Category1", "Category2" },
                Status = null
            };

            // Act
            var result = criteria.ToSearchParams();

            // Assert
            result.Parameters.Should().HaveCount(1);
            result.Parameters.Should().Contain(p => p.Item1 == "category" && p.Item2 == "category1,category2");
        }
    }
}
// <copyright file="EncounterSearchCriteriaTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Exceptions;
using FluentAssertions;
using Hl7.Fhir.Support;
using NUnit.Framework;
using System;

namespace Fhir.Adapter.Tests.Contracts
{
    [TestFixture]
    public class EncounterSearchCriteriaTests
    {
        [Test]
        public void ToSearchParamsTest_PatientId_ArgumentNullException_Test()
        {
            //Arrange
            var encounterSearchCriteria = new EncounterSearchCriteria();

            //Act
            Action action = () => encounterSearchCriteria.ToSearchParams();

            //Assert
            var result = action.Should().Throw<MandatoryQueryParameterMissingException>();
            result.Which.Message.Should().Be("PatientId is required");
        }

        [Test]
        public void ToSearchParamsTest_PatientId_Test()
        {
            //Arrange
            var encounterSearchCriteria = new EncounterSearchCriteria { PatientId = Guid.NewGuid().ToString() };

            //Act
            var result = encounterSearchCriteria.ToSearchParams();

            //Assert
            result.Parameters.Should().Contain(p =>
                p.Item1 == "patient" && p.Item2 == $"Patient/{encounterSearchCriteria.PatientId}");
        }

        [Test]
        public void ToSearchParamsTest_StartDateTime_Test()
        {
            //Arrange
            var encounterSearchCriteria = new EncounterSearchCriteria
            {
                PatientId = Guid.NewGuid().ToString(),
                StartDateTime = DateTime.UtcNow
            };
            var fhirDateTime = encounterSearchCriteria.StartDateTime?.ToFhirDateTime(TimeSpan.Zero);

            //Act
            var result = encounterSearchCriteria.ToSearchParams();

            //Assert
            result.Parameters.Should().Contain(p =>
                p.Item1 == "date" && p.Item2 == $"ge{fhirDateTime}");
        }

        [Test]
        public void ToSearchParamsTest_EndDateTime_Test()
        {
            //Arrange
            var encounterSearchCriteria = new EncounterSearchCriteria
            {
                PatientId = Guid.NewGuid().ToString(),
                StartDateTime = DateTime.UtcNow,
                EndDateTime = DateTime.UtcNow
            };
            var fhirDateTime = encounterSearchCriteria.EndDateTime?.ToFhirDateTime(TimeSpan.Zero);

            //Act
            var result = encounterSearchCriteria.ToSearchParams();

            //Assert
            result.Parameters.Should().Contain(p =>
                p.Item1 == "date" && p.Item2 == $"le{fhirDateTime}");
        }

        [Test]
        public void ToSearchParamsTest_EndDateTime_ContradictoryQueryException_Test()
        {
            //Arrange
            var encounterSearchCriteria = new EncounterSearchCriteria
            {
                PatientId = Guid.NewGuid().ToString(),
                EndDateTime = DateTime.UtcNow.AddDays(-12),
                StartDateTime = DateTime.UtcNow
            };

            //Act
            Action action = () => encounterSearchCriteria.ToSearchParams();

            //Assert
            var result = action.Should().Throw<ContradictoryQueryException>();
            result.Which.Message.Should().Be("Encounter query EndDateTime < StartDateTime");
        }
    }
}

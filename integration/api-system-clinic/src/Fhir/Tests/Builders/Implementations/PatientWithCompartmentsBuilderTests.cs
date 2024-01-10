// <copyright file="PatientWithCompartmentsBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Builders.Implementations;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Builders.Implementations
{
    [TestFixture]
    public class PatientWithCompartmentsBuilderTests
    {
        [Test]
        public void ExtractPatientWithCompartments_Success_Test()
        {
            //Arrange
            IPatientWithCompartmentsBuilder builder = new PatientWithCompartmentsBuilder();
            var searchResult = new R4.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new R4.Patient
                    {
                        Id = Guid.NewGuid().ToString()
                    }
                }
            };

            //Act
            var result = builder.ExtractPatientWithCompartments(searchResult);

            //Assert
            result.Should().NotBeNull();
            result.Resource.Should().Be(searchResult[0].Resource);
        }

        [Test]
        public void ExtractPatientWithCompartments_ThrowsException_Test()
        {
            //Arrange
            IPatientWithCompartmentsBuilder builder = new PatientWithCompartmentsBuilder();
            var searchResult = new R4.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new R4.Appointment()
                }
            };

            //Act
            Action action = () => builder.ExtractPatientWithCompartments(searchResult);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void ExtractPatientsWithCompartmentsTest()
        {
            //Arrange
            IPatientWithCompartmentsBuilder builder = new PatientWithCompartmentsBuilder();
            var searchResult = new R4.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new R4.Patient
                    {
                        Id = Guid.NewGuid().ToString()
                    }
                }
            };

            //Act
            var result = builder.ExtractPatientsWithCompartments(searchResult);

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Should().NotBeNull();
            result.First().Resource.Should().Be(searchResult[0].Resource);
        }
    }
}
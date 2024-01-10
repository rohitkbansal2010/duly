// <copyright file="DosageConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias stu3;

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;

using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class DosageConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void Convert_CodeableConcept_Throw_ConceptNotMappedException_Test()
        {
            //Arrange
            STU3.Dosage source = new()
            {
                AsNeeded = new Hl7.Fhir.Model.CodeableConcept("sys", "cod"),
                Dose = new Hl7.Fhir.Model.Quantity()
            };

            //Act
            Action action = () => Mapper.Map<Dosage>(source);

            //Assert
            var result = action.Should().Throw<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Could not map AsNeeded");
        }

        [Test]
        public void Convert_doseAndRateIsNotQuantity_Test()
        {
            //Arrange

            STU3.Dosage source = new()
            {
                AsNeeded = new Hl7.Fhir.Model.FhirBoolean(),
                Dose = new Hl7.Fhir.Model.Quantity()
            };

            //Act
            var result = Mapper.Map<Dosage>(source);

            //Assert
            result.Should().NotBeNull();
            result.AsNeeded.Should().BeFalse();
        }

        [Test]
        public void ConvertTest()
        {
            //Arrange
            STU3.Dosage source = new()
            {
                AsNeeded = new Hl7.Fhir.Model.FhirBoolean(),
                Dose = new Hl7.Fhir.Model.Quantity(),
                Timing = new STU3.Timing(),
                PatientInstruction = "Test instruction",
                Text = "Test text"
            };

            //Act
            var result = Mapper.Map<Dosage>(source);

            //Assert
            result.Should().NotBeNull();
            result.AsNeeded.Should().BeFalse();
            result.Timing.Should().NotBeNull();
            result.PatientInstruction.Should().Be(source.PatientInstruction);
            result.Text.Should().Be(source.Text);
        }

        [Test]
        [Ignore("Will be fixed soon")]
        public void ConvertTest_Throw_ConceptNotMappedException()
        {
            //Arrange
            STU3.Dosage source = new()
            {
                Dose = new STU3.Annotation()
            };

            //Act
            Action action = () => Mapper.Map<Dosage>(source);

            //Assert
            var result = action.Should().Throw<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Could not map Dose");
        }
    }
}
// <copyright file="TypedObservationConverterBaseTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Repositories.Mappings.ObservationConverters;
using Duly.Clinic.Contracts;
using FluentAssertions;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System;
using System.Linq;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings.ObservationConverters
{
    [TestFixture]
    public class TypedObservationConverterBaseTests
    {
        private const string SystemLoinc = "http://loinc.org";

        [Test]
        public void ConvertTest()
        {
            var source = new R4.Observation
            {
                Code = new(SystemLoinc, "72514-3"),
                Effective = new FhirDateTime(2017, 01, 20, 0, 0, 0, default),
                Value = new Hl7.Fhir.Model.Quantity { Value = 3, Unit = "score" }
            };

            var converter = new TypedObservationConverterPainLevel();

            //Act
            var result = converter.Convert(source);

            //Assert
            result.Should().NotBeNull();
            result.Date.Should().Be(new DateTimeOffset(2017, 01, 20, 0, 0, 0, default));
            result.Type.Should().Be(ObservationType.PainLevel);
            result.Components.Should().NotBeNullOrEmpty();
            result.Components.Should().HaveCount(1);
            result.Components.First().Type.Should().BeNull();
            result.Components.First().Measurements.First().Value.Should().Be(3M);
            result.Components.First().Measurements.First().Unit.Should().Be("score");
        }

        [Test]
        public void ConvertTest_ThrowsConceptNotMappedException()
        {
            var source = new R4.Observation
            {
                Code = new(default, "101")
            };

            var converter = new TypedObservationConverterBloodPressure();

            //Act
            Action action = () => converter.Convert(source);

            //Assert
            var result = action.Should().ThrowExactly<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Expected coding not found");
        }

        [Test]
        public void ConvertTest_FindDate_ThrowsConceptNotMappedException()
        {
            var source = new R4.Observation
            {
                Code = new(SystemLoinc, "72514-3")
            };

            var converter = new TypedObservationConverterPainLevel();

            //Act
            Action action = () => converter.Convert(source);

            //Assert
            var result = action.Should().ThrowExactly<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Could not map effective of observation");
        }

        [Test]
        public void ConvertTest_FindMeasurementFromQuantity_ThrowsConceptNotMappedException()
        {
            var source = new R4.Observation
            {
                Code = new(SystemLoinc, "72514-3"),
                Effective = new FhirDateTime(2017, 01, 20, 0, 0, 0, default)
            };

            var converter = new TypedObservationConverterPainLevel();

            //Act
            Action action = () => converter.Convert(source);

            //Assert
            var result = action.Should().ThrowExactly<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Unsupported type of DataType for ObservationMeasurement");
        }
    }
}

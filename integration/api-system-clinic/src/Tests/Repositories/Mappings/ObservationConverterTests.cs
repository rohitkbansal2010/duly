// <copyright file="ObservationConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;
using FluentAssertions;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System;
using System.Linq;
using Observation = Duly.Clinic.Contracts.Observation;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class ObservationConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        private const string SystemLoinc = "http://loinc.org";

        [Test]
        public void ConvertTest_BloodPressure()
        {
            //Arrange

            var source = new ObservationWithCompartments
            {
                Resource = new()
                {
                    Effective = new FhirDateTime(2017, 01, 20, 0, 0, 0, default),
                    Code = new CodeableConcept
                    {
                        Coding = new()
                        {
                            new("urn:oid:1.2.246.537.6.96", "8480-6"),
                            new(SystemLoinc, "85354-9"),
                            new(SystemLoinc, "8716-3")
                        }
                    },
                    Component = new()
                    {
                        new()
                        {
                            Code = new CodeableConcept(SystemLoinc, "8480-6"),
                            Value = new Hl7.Fhir.Model.Quantity { Value = 120M, Unit = "mm[Hg]" },
                        },
                        new()
                        {
                            Code = new CodeableConcept(SystemLoinc, "8462-4"),
                            Value = new Hl7.Fhir.Model.Quantity { Value = 80M, Unit = "mm[Hg]" }
                        }
                    },
                }
            };

            //Act
            var result = Mapper.Map<Observation>(source);

            //Assert
            result.Should().NotBeNull();
            result.Date.Should().Be(new DateTimeOffset(2017, 01, 20, 0, 0, 0, default));
            result.Type.Should().Be(ObservationType.BloodPressure);
            result.Components.Should().NotBeEmpty();
            result.Components.Should().HaveCount(2);
            result.Components.First().Should().NotBeNull();
            result.Components.First().Type.Should().Be(ObservationComponentType.Systolic);
            result.Components.First().Measurements.Should().NotBeEmpty();
            result.Components.First().Measurements.First().Should().NotBeNull();
            result.Components.First().Measurements.First().Value.Should().Be(120M);
            result.Components.First().Measurements.First().Unit.Should().Be("mm[Hg]");
        }

        [Test]
        public void ConvertTest_BodyTemperature()
        {
            //Arrange
            var source = new ObservationWithCompartments
            {
                Resource = new()
                {
                    Effective = new FhirDateTime(2017, 01, 20, 0, 0, 0, default),
                    Code = new CodeableConcept
                    {
                        Coding = new()
                        {
                            new("urn:oid:1.2.246.537.6.96", "8480-6"),
                            new(SystemLoinc, "8310-5"),
                            new(SystemLoinc, "8716-3")
                        }
                    },
                    Value = new Hl7.Fhir.Model.Quantity(36.4M, "Cel"),
                    Extension = new()
                    {
                        new()
                        {
                            Value = new Hl7.Fhir.Model.Quantity
                            {
                                Value = 97.6M,
                                Unit = "[degF]",
                                Code = "[degF]",
                                System = "http://unitsofmeasure.org",
                            },
                            Url = "http://open.epic.com/FHIR/STU3/StructureDefinition/temperature-in-fahrenheit"
                        }
                    }
                }
            };

            //Act
            var result = Mapper.Map<Observation>(source);

            //Assert
            result.Should().NotBeNull();
            result.Date.Should().Be(new DateTimeOffset(2017, 01, 20, 0, 0, 0, default));
            result.Type.Should().Be(ObservationType.BodyTemperature);
            result.Components.Should().NotBeEmpty();
            result.Components.Should().HaveCount(1);
            result.Components.First().Should().NotBeNull();
            result.Components.First().Type.Should().BeNull();
            result.Components.First().Measurements.Should().NotBeEmpty();

            result.Components.First().Measurements[0].Should().NotBeNull();
            result.Components.First().Measurements[0].Value.Should().Be(36.4M);
            result.Components.First().Measurements[0].Unit.Should().Be("Cel");

            result.Components.First().Measurements[1].Should().NotBeNull();
            result.Components.First().Measurements[1].Value.Should().Be(97.6M);
            result.Components.First().Measurements[1].Unit.Should().Be("[degF]");
        }

        [TestCase("2708-6", ObservationType.OxygenSaturation)]
        [TestCase("59408-5", ObservationType.OxygenSaturation)]
        [TestCase("8867-4", ObservationType.HeartRate)]
        [TestCase("9279-1", ObservationType.RespiratoryRate)]
        [TestCase("72514-3", ObservationType.PainLevel)]
        [TestCase("29463-7", ObservationType.BodyWeight)]
        [TestCase("8302-2", ObservationType.BodyHeight)]
        [TestCase("39156-5", ObservationType.BodyMassIndex)]

        public void ConvertTest_OtherTypes(string code, ObservationType expected)
        {
            //Arrange
            var source = new ObservationWithCompartments
            {
                Resource = new()
                {
                    Effective = new FhirDateTime(2017, 01, 20, 0, 0, 0, default),
                    Code = new CodeableConcept
                    {
                        Coding = new()
                        {
                            new("urn:oid:1.2.246.537.6.96", "8480-6"),
                            new(SystemLoinc, code),
                            new(SystemLoinc, "8716-3")
                        }
                    },
                    Value = new Hl7.Fhir.Model.Quantity(36.4M, "Cel"),
                }
            };

            //Act
            var result = Mapper.Map<Observation>(source);

            //Assert
            result.Should().NotBeNull();
            result.Date.Should().Be(new DateTimeOffset(2017, 01, 20, 0, 0, 0, default));
            result.Type.Should().Be(expected);
            result.Components.Should().NotBeEmpty();
            result.Components.Should().HaveCount(1);
            result.Components.First().Should().NotBeNull();
            result.Components.First().Type.Should().BeNull();
            result.Components.First().Measurements.Should().NotBeEmpty();
            result.Components.First().Measurements.First().Should().NotBeNull();
            result.Components.First().Measurements.First().Value.Should().Be(36.4M);
            result.Components.First().Measurements.First().Unit.Should().Be("Cel");
        }

        [Test]
        public void ConvertTest_ThrowsConceptNotMappedException_WrongTypeCode()
        {
            //Arrange
            var source = new ObservationWithCompartments
            {
                Resource = new()
                {
                    Effective = new FhirDateTime(2017, 01, 20, 0, 0, 0, default),
                    Code = new CodeableConcept
                    {
                        Coding = new()
                        {
                            new("urn:oid:1.2.246.537.6.96", "8480-6"),
                            new(SystemLoinc, "18480-6"),
                        }
                    },
                }
            };

            //Act
            Action action = () => Mapper.Map<Observation>(source);

            //Assert
            var result = action.Should().ThrowExactly<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Could not map codes to a ObservationType");
        }

        [Test]
        public void ConvertTest_ThrowsConceptNotMappedException_TooLowComponents()
        {
            //Arrange
            var source = new ObservationWithCompartments
            {
                Resource = new()
                {
                    Effective = new FhirDateTime(2017, 01, 20, 0, 0, 0, default),
                    Code = new CodeableConcept
                    {
                        Coding = new()
                        {
                            new("urn:oid:1.2.246.537.6.96", "8480-6"),
                            new(SystemLoinc, "85354-9"),
                        }
                    },
                }
            };

            //Act
            Action action = () => Mapper.Map<Observation>(source);

            //Assert
            var result = action.Should().ThrowExactly<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Component Count is too low");
        }

        [Test]
        public void ConvertTest_ThrowsConceptNotMappedException_NoEffectiveDate()
        {
            //Arrange
            var source = new ObservationWithCompartments
            {
                Resource = new()
                {
                    Code = new CodeableConcept
                    {
                        Coding = new()
                        {
                            new("urn:oid:1.2.246.537.6.96", "8480-6"),
                            new(SystemLoinc, "2708-6"),
                        }
                    },
                }
            };

            //Act
            Action action = () => Mapper.Map<Observation>(source);

            //Assert
            var result = action.Should().ThrowExactly<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Could not map effective of observation");
        }

        [Test]
        public void ConvertTest_ThrowsConceptNotMappedException_WrongComponentTypeCode()
        {
            //Arrange
            var source = new ObservationWithCompartments
            {
                Resource = new()
                {
                    Effective = new FhirDateTime(2017, 01, 20, 0, 0, 0, default),
                    Code = new CodeableConcept
                    {
                        Coding = new()
                        {
                            new("urn:oid:1.2.246.537.6.96", "8480-6"),
                            new(SystemLoinc, "85354-9"),
                        }
                    },
                    Component = new()
                    {
                        new()
                        {
                            Code = new CodeableConcept(SystemLoinc, "18480-6"),
                        },
                        new()
                        {
                            Code = new CodeableConcept(SystemLoinc, "8480-6"),
                        },
                    },
                }
            };

            //Act
            Action action = () => Mapper.Map<Observation>(source);

            //Assert
            var result = action.Should().ThrowExactly<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Could not map type of observation component");
        }
    }
}

// <copyright file="TypedObservationConverterBloodPressureTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;
using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Repositories.Mappings.ObservationConverters;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;
using r4::Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using CodeableConcept = Hl7.Fhir.Model.CodeableConcept;
using Coding = Hl7.Fhir.Model.Coding;
using FhirDateTime = Hl7.Fhir.Model.FhirDateTime;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings.ObservationConverters
{
    [TestFixture]
    public class TypedObservationConverterBloodPressureTests
    {
        [Test]
        public void ConvertTest_ComponentsTest()
        {
            var converter = new TypedObservationConverterBloodPressure();
            var observation = new Observation
            {
                Code = new CodeableConcept()
                {
                    Coding = new List<Coding>()
                    {
                        new Coding("http://loinc.org", "85354-9")
                    }
                },
                Effective = new FhirDateTime(DateTime.Now.ToUniversalTime().ToDateTimeOffset())
            };

            converter.Invoking(x => x.Convert(observation)).Should().Throw<ConceptNotMappedException>()
                .WithMessage("Component Count is too low");
        }

        [Test]
        public void ConvertTest_ComponentsTest_MissingCoding()
        {
            var converter = new TypedObservationConverterBloodPressure();
            var observation = new Observation()
                {
                    Effective = new FhirDateTime(2017, 01, 20, 0, 0, 0, default),
                    Code = new CodeableConcept
                    {
                        Coding = new()
                        {
                            new("urn:oid:1.2.246.537.6.96", "8480-6"),
                            new("http://loinc.org", "85354-9"),
                            new("http://loinc.org", "8716-3")
                        }
                    },
                    Component = new()
                    {
                        new()
                        {
                            //Code = new CodeableConcept("http://loinc.org", "8480-6"),
                            Value = new Hl7.Fhir.Model.Quantity { Value = 120M, Unit = "mm[Hg]" },
                        },
                        new()
                        {
                            Code = new CodeableConcept("http://loinc.org", "8462-4"),
                            Value = new Hl7.Fhir.Model.Quantity { Value = 80M, Unit = "mm[Hg]" }
                        }
                    }
                };

            converter.Invoking(x => x.Convert(observation)).Should().Throw<ConceptNotMappedException>()
                .WithMessage("Could not map type of observation component");
        }

        [Test]
        public void ConvertTest_ComponentsTest_MissingWrongCoding()
        {
            var converter = new TypedObservationConverterBloodPressure();
            var observation = new Observation()
            {
                Effective = new FhirDateTime(2017, 01, 20, 0, 0, 0, default),
                Code = new CodeableConcept
                {
                    Coding = new()
                    {
                        new("urn:oid:1.2.246.537.6.96", "8480-6"),
                        new("http://loinc.org", "85354-9"),
                        new("http://loinc.org", "8716-3")
                    }
                },
                Component = new()
                {
                    new()
                    {
                        Code = new CodeableConcept("http://loinc.org", "XXXXX"),
                        Value = new Hl7.Fhir.Model.Quantity { Value = 120M, Unit = "mm[Hg]" },
                    },
                    new()
                    {
                        Code = new CodeableConcept("http://loinc.org", "8462-4"),
                        Value = new Hl7.Fhir.Model.Quantity { Value = 80M, Unit = "mm[Hg]" }
                    }
                }
            };

            converter.Invoking(x => x.Convert(observation)).Should().Throw<ConceptNotMappedException>()
                .WithMessage("Could not map type of observation component");
        }

        [Test]
        public void ConvertTest_ComponentsTest_Correct()
        {
            var converter = new TypedObservationConverterBloodPressure();
            var observation = new Observation()
            {
                Effective = new FhirDateTime(2017, 01, 20, 0, 0, 0, default),
                Code = new CodeableConcept
                {
                    Coding = new()
                    {
                        new("urn:oid:1.2.246.537.6.96", "8480-6"),
                        new("http://loinc.org", "85354-9"),
                        new("http://loinc.org", "8716-3")
                    }
                },
                Component = new()
                {
                    new()
                    {
                        Code = new CodeableConcept("http://loinc.org", "8480-6"),
                        Value = new Hl7.Fhir.Model.Quantity { Value = 120M, Unit = "mm[Hg]" },
                    },
                    new()
                    {
                        Code = new CodeableConcept("http://loinc.org", "8462-4"),
                        Value = new Hl7.Fhir.Model.Quantity { Value = 80M, Unit = "mm[Hg]" }
                    }
                }
            };

            converter.Invoking(x => x.Convert(observation)).Should().NotThrow();
        }
    }
}

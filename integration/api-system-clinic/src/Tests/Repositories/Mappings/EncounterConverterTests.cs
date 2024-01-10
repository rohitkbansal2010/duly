// <copyright file="EncounterConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class EncounterConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [TestCase("IMP", EncounterType.OnSite)]
        [TestCase("VR", EncounterType.Telehealth)]
        [TestCase("code", EncounterType.OnSite)]
        public void ConvertTest(string classCode, EncounterType expectedType)
        {
            //Arrange
            EncounterWithCompartments source = new()
            {
                Resource = new R4.Encounter
                {
                    Id = Guid.NewGuid().ToString(),
                    Class = new Hl7.Fhir.Model.Coding("system", classCode),
                    ServiceType = new Hl7.Fhir.Model.CodeableConcept("testSystem", "testCode", "testText")
                },
                DoesPatientHavePastVisits = true,
                Patient = new R4.Patient
                {
                    Name = new List<R4.HumanName>
                    {
                        new ()
                        {
                            Family = "sss"
                        }
                    }
                },
                Practitioners = new[]
                {
                    new PractitionerWithRoles
                    {
                        Resource = new R4.Practitioner
                        {
                            Name = new List<R4.HumanName>
                            {
                                new ()
                                {
                                    Family = "sss"
                                }
                            }
                        },
                        Roles = new R4.PractitionerRole[]
                        {
                            new ()
                            {
                                Code = new List<Hl7.Fhir.Model.CodeableConcept>
                                {
                                    new ("a", "b", "textTest")
                                }
                            }
                        }
                    }
                },
            };

            //Act
            var result = Mapper.Map<Encounter>(source);

            //Assert
            result.Should().NotBeNull();
            result.ServiceType.Should().Be("testText");
            result.Patient.HasPastVisits.Should().BeTrue();

            result.Practitioner.Names.First().FamilyName.Should().Be("sss");
            result.Practitioner.Roles.Length.Should().Be(1);
            result.Practitioner.Roles[0].Title.Should().Be("textTest");
            result.Type.Should().Be(expectedType);
        }
    }
}

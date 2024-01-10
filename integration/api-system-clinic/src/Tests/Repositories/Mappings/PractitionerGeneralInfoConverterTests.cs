// <copyright file="PractitionerGeneralInfoConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
extern alias stu3;

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
using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class PractitionerGeneralInfoConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertTest_WithEmptyRoles()
        {
            //Arrange
            PractitionerWithRoles source = new()
            {
                Resource = new R4.Practitioner
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = new List<R4.HumanName>
                    {
                        new ()
                        {
                            Use = R4.HumanName.NameUse.Official,
                        }
                    }
                },
                Roles = Array.Empty<R4.PractitionerRole>()
            };

            //Act
            var result = Mapper.Map<PractitionerGeneralInfo>(source);

            //Assert
            result.Should().NotBeNull();
            result.Roles.Should().BeNull();
        }

        [Test]
        public void ConvertTest_WithRoles_R4()
        {
            //Arrange
            PractitionerWithRoles source = new()
            {
                Resource = new R4.Practitioner
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = new List<R4.HumanName>
                    {
                        new ()
                        {
                            Use = R4.HumanName.NameUse.Official
                        }
                    }
                },
                Roles = new R4.PractitionerRole[]
                {
                    new ()
                    {
                        Code = new List<Hl7.Fhir.Model.CodeableConcept>
                        {
                            new ("testSystem", "testCode", "testText")
                        }
                    }
                }
            };

            //Act
            var result = Mapper.Map<PractitionerGeneralInfo>(source);

            //Assert
            result.Should().NotBeNull();
            result.Roles.Should().NotBeEmpty();
            result.Roles.Length.Should().Be(1);
            result.Roles.First().Title.Should().Be("testText");
        }

        [Test]
        public void ConvertTest_WithRoles_STU3()
        {
            //Arrange
            PractitionerWithRolesSTU3 source = new()
            {
                Resource = new STU3.Practitioner
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = new List<STU3.HumanName>
                    {
                        new ()
                        {
                            Use = STU3.HumanName.NameUse.Official
                        }
                    }
                },
                Roles = new STU3.PractitionerRole[]
                {
                    new ()
                    {
                        Code = new List<Hl7.Fhir.Model.CodeableConcept>
                        {
                            new ("testSystem", "testCode", "testText")
                        }
                    }
                }
            };

            //Act
            var result = Mapper.Map<PractitionerGeneralInfo>(source);

            //Assert
            result.Should().NotBeNull();
            result.Roles.Should().NotBeEmpty();
            result.Roles.Length.Should().Be(1);
            result.Roles.First().Title.Should().Be("testText");
        }
    }
}

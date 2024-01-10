// <copyright file="PatientConverterTests.cs" company="Duly Health and Care">
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
    public class PatientConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertTest()
        {
            //Arrange
            var patientId = Guid.NewGuid().ToString();

            PatientWithCompartments source = new()
            {
                Resource = new()
                {
                    Id = patientId,
                    BirthDate = "1950-07-25",
                    Gender = R4.AdministrativeGender.Male,
                    Name = new List<R4.HumanName>
                    {
                        new() { Family = "Smith" }
                    }
                },
            };

            //Act
            var result = Mapper.Map<Patient>(source);

            //Assert
            result.Should().NotBeNull();
            result.BirthDate.Date.Should().Be(new DateTime(1950, 07, 25));
            result.Gender.Should().Be(Gender.Male);
            result.PatientGeneralInfo.Id.Should().Be(patientId);
            result.PatientGeneralInfo.Names.First().FamilyName.Should().Be("Smith");
        }

        [Test]
        public void ConvertTest_BirthDate_Throw_ConceptNotMappedException()
        {
            //Arrange
            PatientWithCompartments source = new()
            {
                Resource = new()
            };

            //Act
            Action action = () => Mapper.Map<Patient>(source);

            //Assert
            var result = action.Should().Throw<ConceptNotMappedException>();
            result.Which.Message.Should().Be("BirthDate is a mandatory field");
        }

        [Test]
        public void ConvertTest_Gender_Throw_ConceptNotMappedException()
        {
            //Arrange
            PatientWithCompartments source = new()
            {
                Resource = new()
                {
                    BirthDate = "test"
                }
            };

            //Act
            Action action = () => Mapper.Map<Patient>(source);

            //Assert
            var result = action.Should().Throw<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Gender is a mandatory field");
        }
    }
}

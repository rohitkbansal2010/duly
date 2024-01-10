// <copyright file="PatientGeneralInfoConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class PatientGeneralInfoConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertTest()
        {
            //Act
            var source = new R4.Patient
            {
                Id = Guid.NewGuid().ToString(),
                Name = new List<R4.HumanName>
                {
                    new ()
                    {
                        Family = "sss"
                    }
                }
            };

            //Arrange
            var result = Mapper.Map<PatientGeneralInfo>(source);

            //Assert
            result.Id.Should().Be(source.Id);
            result.Names.First().FamilyName.Should().Be("sss");
        }
    }
}

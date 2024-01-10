// <copyright file="RelatedPersonGeneralInfoConverterTests.cs" company="Duly Health and Care">
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
    public class RelatedPersonGeneralInfoConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertTest()
        {
            //Arrange
            RelatedPersonWithCompartments source = new()
            {
                Resource = new R4.RelatedPerson
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = new List<R4.HumanName>
                    {
                        new ()
                        {
                            Use = R4.HumanName.NameUse.Official,
                            Family = "Smith"
                        }
                    }
                }
            };

            //Act
            var result = Mapper.Map<RelatedPersonGeneralInfo>(source);

            //Assert
            result.Should().NotBeNull();
            result.Names.First().Should().NotBeNull();
            result.Names.First().Should().BeOfType<HumanName>();
            result.Names.First().FamilyName.Should().Be("Smith");
        }
    }
}

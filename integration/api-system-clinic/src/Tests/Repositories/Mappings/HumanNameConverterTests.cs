// <copyright file="HumanNameConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using NUnit.Framework;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class HumanNameConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertTest()
        {
            //Arrange
            R4.HumanName source = new()
            {
                Family = "testFamily",
                Given = new[] { "a", "b" },
                Prefix = new[] { "aaa" },
                Suffix = new[] { "ssss", "asdf" }
            };

            //Act
            var result = Mapper.Map<HumanName>(source);

            //Assert
            result.Should().NotBeNull();
            result.FamilyName.Should().Be("testFamily");
            result.GivenNames.Length.Should().Be(2);
            result.Prefixes.Length.Should().Be(1);
            result.Suffixes.Length.Should().Be(2);

            result.GivenNames[0].Should().Be("a");
            result.GivenNames[1].Should().Be("b");
            result.Prefixes[0].Should().Be("aaa");
            result.Suffixes[0].Should().Be("ssss");
            result.Suffixes[1].Should().Be("asdf");
        }
    }
}

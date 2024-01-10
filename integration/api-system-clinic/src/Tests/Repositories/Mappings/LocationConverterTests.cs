// <copyright file="LocationConverterTests.cs" company="Duly Health and Care">
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
    public class LocationConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertTest()
        {
            //Arrange
            R4.Encounter.LocationComponent source = new()
            {
                Location = new Hl7.Fhir.Model.ResourceReference("testRef", "testDisplay")
            };

            //Act
            var result = Mapper.Map<Location>(source);

            //Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("testDisplay");
        }
    }
}

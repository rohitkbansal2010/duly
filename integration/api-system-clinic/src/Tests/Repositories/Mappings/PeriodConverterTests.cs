// <copyright file="PeriodConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class PeriodConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertNullTest()
        {
            //Arrange
            Hl7.Fhir.Model.Period source = new();

            //Act
            var result = Mapper.Map<Period>(source);

            //Assert
            result.Should().NotBeNull();
            result.Start.Should().BeNull();
            result.End.Should().BeNull();
        }

        [Test]
        public void ConvertValueTest()
        {
            //Arrange
            var start = new DateTimeOffset(2021, 12, 21, 0, 0, 0, 0, default);
            var end = new DateTimeOffset(2021, 12, 22, 0, 0, 0, 0, default);

            Hl7.Fhir.Model.Period source = new()
            {
                StartElement = new Hl7.Fhir.Model.FhirDateTime(start),
                EndElement = new Hl7.Fhir.Model.FhirDateTime(end)
            };

            //Act
            var result = Mapper.Map<Period>(source);

            //Assert
            result.Should().NotBeNull();
            result.Start.Should().Be(start);
            result.End.Should().Be(end);
        }
    }
}
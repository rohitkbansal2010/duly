// <copyright file="EncounterStatusConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class EncounterStatusConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [TestCase(R4.Encounter.EncounterStatus.Arrived, EncounterStatus.Arrived)]
        [TestCase(R4.Encounter.EncounterStatus.InProgress, EncounterStatus.InProgress)]
        [TestCase(R4.Encounter.EncounterStatus.Planned, EncounterStatus.Planned)]
        [TestCase(R4.Encounter.EncounterStatus.Cancelled, EncounterStatus.Cancelled)]
        [TestCase(R4.Encounter.EncounterStatus.Finished, EncounterStatus.Finished)]
        public void ConvertTest(R4.Encounter.EncounterStatus source, EncounterStatus expected)
        {
            //Arrange

            //Act
            var result = Mapper.Map<EncounterStatus>(source);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ConvertTest_ThrowsConceptNotMappedException()
        {
            //Arrange
            var source = R4.Encounter.EncounterStatus.Triaged;

            //Act
            Action action = () => Mapper.Map<EncounterStatus>(source);

            //Assert
            var result = action.Should().ThrowExactly<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Could not map EncounterStatus to System model");
        }
    }
}

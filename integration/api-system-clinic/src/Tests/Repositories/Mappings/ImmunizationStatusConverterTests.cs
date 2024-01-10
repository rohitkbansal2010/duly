// <copyright file="ImmunizationStatusConverterTests.cs" company="Duly Health and Care">
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
    public class ImmunizationStatusConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [TestCase(R4.Immunization.ImmunizationStatusCodes.NotDone, ImmunizationStatus.NotDone)]
        [TestCase(R4.Immunization.ImmunizationStatusCodes.Completed, ImmunizationStatus.Completed)]
        [TestCase(R4.Immunization.ImmunizationStatusCodes.EnteredInError, ImmunizationStatus.EnteredInError)]
        public void ConvertTest(R4.Immunization.ImmunizationStatusCodes immunizationStatus, ImmunizationStatus expectedImmunizationStatus)
        {
            //Act
            var result = Mapper.Map<ImmunizationStatus>(immunizationStatus);

            //Assert
            result.Should().Be(expectedImmunizationStatus);
        }

        [Test]
        public void UnknownStatus_ThrowsException_Test()
        {
            //Assert
            const R4.Immunization.ImmunizationStatusCodes status = (R4.Immunization.ImmunizationStatusCodes)999;

            //Act and Assert
            status.Invoking(s => Mapper.Map<ImmunizationStatus>(s)).Should().Throw<ConceptNotMappedException>();
        }
    }
}
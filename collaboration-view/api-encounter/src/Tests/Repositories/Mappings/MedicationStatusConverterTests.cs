// <copyright file="MedicationStatusConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Repositories.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    internal class MedicationStatusConverterTests : MapperConfigurator<SystemContractsToRepositoryModelsProfile>
    {
        [TestCase(MedicationStatus.Active, Models.MedicationStatus.Active)]
        [TestCase(MedicationStatus.Inactive, Models.MedicationStatus.Inactive)]
        [TestCase(MedicationStatus.EnteredInError, Models.MedicationStatus.EnteredInError)]
        public void ConvertTest(MedicationStatus source, Models.MedicationStatus expected)
        {
            //Arrange

            //Act
            var result = Mapper.Map<Models.MedicationStatus>(source);

            //Assert
            result.Should().Be(expected);
        }
    }
}

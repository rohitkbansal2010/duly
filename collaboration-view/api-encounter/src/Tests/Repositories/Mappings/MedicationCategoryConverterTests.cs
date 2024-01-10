// <copyright file="MedicationCategoryConverterTests.cs" company="Duly Health and Care">
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
    internal class MedicationCategoryConverterTests : MapperConfigurator<SystemContractsToRepositoryModelsProfile>
    {
        [TestCase(MedicationCategory.Inpatient, Models.MedicationCategory.Inpatient)]
        [TestCase(MedicationCategory.Outpatient, Models.MedicationCategory.Outpatient)]
        [TestCase(MedicationCategory.Community, Models.MedicationCategory.Community)]
        [TestCase(MedicationCategory.PatientSpecified, Models.MedicationCategory.PatientSpecified)]
        public void ConvertTest(MedicationCategory source, Models.MedicationCategory expected)
        {
            //Arrange

            //Act
            var result = Mapper.Map<Models.MedicationCategory>(source);

            //Assert
            result.Should().Be(expected);
        }
    }
}

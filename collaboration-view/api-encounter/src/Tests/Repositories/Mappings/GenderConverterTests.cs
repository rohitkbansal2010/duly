// <copyright file="GenderConverterTests.cs" company="Duly Health and Care">
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
    internal class GenderConverterTests : MapperConfigurator<SystemContractsToRepositoryModelsProfile>
    {
        [TestCase(Gender.Unknown, Models.Gender.Unknown)]
        [TestCase(Gender.Other, Models.Gender.Other)]
        [TestCase(Gender.Male, Models.Gender.Male)]
        [TestCase(Gender.Female, Models.Gender.Female)]
        public void ConvertTest(Gender sourceGender, Models.Gender targetGender)
        {
            //Arrange

            //Act
            var result = Mapper.Map<Models.Gender>(sourceGender);

            //Assert
            result.Should().Be(targetGender);
        }
    }
}

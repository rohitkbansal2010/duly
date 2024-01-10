// <copyright file="GenderConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using System;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    internal class GenderConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [TestCase(Gender.Unknown, ApiContracts.Gender.Unknown)]
        [TestCase(Gender.Other, ApiContracts.Gender.Other)]
        [TestCase(Gender.Male, ApiContracts.Gender.Male)]
        [TestCase(Gender.Female, ApiContracts.Gender.Female)]
        public void ConvertTest(Gender sourceGender, ApiContracts.Gender targetGender)
        {
            //Arrange

            //Act
            var result = Mapper.Map<ApiContracts.Gender>(sourceGender);

            //Assert
            result.Should().Be(targetGender);
        }

        [Test]
        public void ConvertTest_Throw_ServiceNotMappedException()
        {
            //Arrange
            var source = (Gender)(-1);

            //Act
            Action action = () => Mapper.Map<ApiContracts.Gender>(source);

            //Assert
            var result = action.Should().ThrowExactly<ServiceNotMappedException>();
            result.Which.Message.Should().Be("Could not map Gender to contract");
        }
    }
}

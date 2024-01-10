// <copyright file="AdapterUiConfigurationTargetAreaTypeConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Resource.Api.Contracts;
using Duly.CollaborationView.Resource.Api.Repositories.Mappings;
using Duly.Resource.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using System;
using AdapterContracts = Duly.UiConfig.Adapter.Contracts;

namespace Duly.Resource.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    internal class AdapterUiConfigurationTargetAreaTypeConverterTests : MapperConfigurator<AdapterContractsToProcessContractsProfile>
    {
        [TestCase(AdapterContracts.ApplicationPart.CurrentAppointment, ApplicationPart.CurrentAppointment)]
        [TestCase(AdapterContracts.ApplicationPart.Calendar, ApplicationPart.Calendar)]
        public void ConvertTest(AdapterContracts.ApplicationPart source, ApplicationPart expected)
        {
            //Act
            var result = Mapper.Map<ApplicationPart>(source);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ConvertTest_Throw_RepositoryNotMappedException()
        {
            //Arrange
            var source = (AdapterContracts.ApplicationPart)(-1);

            //Act
            Action action = () => Mapper.Map<ApplicationPart>(source);

            //Assert
            var result = action.Should().ThrowExactly<RepositoryNotMappedException>();
            result.Which.Message.Should().Be("Could not map ApplicationPart to corresponding contract.");
        }
    }
}

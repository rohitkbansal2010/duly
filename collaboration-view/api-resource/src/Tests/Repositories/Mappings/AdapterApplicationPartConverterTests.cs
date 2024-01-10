// <copyright file="AdapterApplicationPartConverterTests.cs" company="Duly Health and Care">
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
    internal class AdapterApplicationPartConverterTests : MapperConfigurator<AdapterContractsToProcessContractsProfile>
    {
        [TestCase(AdapterContracts.UiConfigurationTargetAreaType.Navigation, UiConfigurationTargetAreaType.Navigation)]
        [TestCase(AdapterContracts.UiConfigurationTargetAreaType.Layout, UiConfigurationTargetAreaType.Layout)]
        public void ConvertTest(AdapterContracts.UiConfigurationTargetAreaType source, UiConfigurationTargetAreaType expected)
        {
            //Act
            var result = Mapper.Map<UiConfigurationTargetAreaType>(source);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ConvertTest_Throw_RepositoryNotMappedException()
        {
            //Arrange
            var source = (AdapterContracts.UiConfigurationTargetAreaType)(-1);

            //Act
            Action action = () => Mapper.Map<UiConfigurationTargetAreaType>(source);

            //Assert
            var result = action.Should().ThrowExactly<RepositoryNotMappedException>();
            result.Which.Message.Should().Be("Could not map UiConfigurationTargetAreaType to corresponding contract.");
        }
    }
}

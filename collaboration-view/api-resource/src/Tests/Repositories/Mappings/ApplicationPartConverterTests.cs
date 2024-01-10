// <copyright file="ApplicationPartConverterTests.cs" company="Duly Health and Care">
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
    internal class ApplicationPartConverterTests : MapperConfigurator<ProcessContractsToAdapterContractsProfile>
    {
        [TestCase(ApplicationPart.CurrentAppointment, AdapterContracts.ApplicationPart.CurrentAppointment)]
        [TestCase(ApplicationPart.Calendar, AdapterContracts.ApplicationPart.Calendar)]
        public void ConvertTest(ApplicationPart source, AdapterContracts.ApplicationPart expected)
        {
            //Act
           var result = Mapper.Map<AdapterContracts.ApplicationPart>(source);

            //Assert
           result.Should().Be(expected);
        }

        [Test]
        public void ConvertTest_Throw_RepositoryNotMappedException()
        {
            //Arrange
            var source = (ApplicationPart)(-1);

            //Act
            Action action = () => Mapper.Map<AdapterContracts.ApplicationPart>(source);

            //Assert
            var result = action.Should().ThrowExactly<RepositoryNotMappedException>();
            result.Which.Message.Should().Be("Could not map ApplicationPart to corresponding adapter contract.");
        }
    }
}

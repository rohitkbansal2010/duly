// <copyright file="ModuleInitializerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Configurations;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;

namespace Duly.CollaborationView.Encounter.Api.Tests.Configurations
{
    [TestFixture]
    public class ModuleInitializerTests
    {
        private IServiceCollection _services;
        private IConfiguration _configuration;

        [SetUp]
        public void SetUp()
        {
            _configuration = new Mock<IConfiguration>().Object;
            _services = new ServiceCollection();
        }

        [Test]
        public void AddApiMappingsTest()
        {
            //Arrange
            _services.AddApiMappings(_configuration);

            var provider = _services.BuildServiceProvider();

            //Act
            var mapper = provider.GetService<IMapper>();
            Action validation = () => mapper.ConfigurationProvider.AssertConfigurationIsValid();

            //Assert
            validation.Should().NotThrow();
        }
    }
}

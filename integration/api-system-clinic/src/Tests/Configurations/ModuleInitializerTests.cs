// <copyright file="ModuleInitializerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Configurations;
using Duly.Common.Infrastructure.Components;
using Duly.Common.Security.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Net;

namespace Duly.Clinic.Api.Tests.Configurations
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

        [Test]
        public void ExtendErrorHandlingOptionsTest()
        {
            //Arrange
            var options = new ErrorHandlingOptions();

            //Act
            ModuleInitializer.ExtendErrorHandlingOptions(options);

            //Assert
            options.ResultStatusCodesAndMessages.Should().ContainKey(typeof(ObfuscatorException));
            options.ResultStatusCodesAndMessages[typeof(ObfuscatorException)].statusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}

// <copyright file="LogAnalyticsExtensionsTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Audit.Ingestion.Extensions;
using FluentAssertions;
using LogAnalytics.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace Duly.Clinic.Audit.Ingestion.Tests
{
    [TestFixture]
    public class LogAnalyticsExtensionsTests
    {
        private IServiceCollection _services;
        private Mock<IConfiguration> _configurationMocked;

        [SetUp]
        public void SetUp()
        {
            _configurationMocked = new Mock<IConfiguration>();
            _services = new ServiceCollection();
        }

        [Test]
        public void AddLogAnalytics()
        {
            //Arrange
            _configurationMocked
                .Setup(x => x.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            //Act
            _services.AddLogAnalytics(_configurationMocked.Object);

            //Assert
            var descriptor = _services.FirstOrDefault(x => x.ServiceType == typeof(ILogAnalyticsClient));
            descriptor.Should().NotBeNull();
        }
    }
}

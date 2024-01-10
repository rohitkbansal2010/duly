// <copyright file="ConfigurationsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Resource.Api.Contracts;
using Duly.CollaborationView.Resource.Api.Controllers;
using Duly.CollaborationView.Resource.Api.Repositories.Interfaces;
using Duly.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Resource.Api.Tests.Controllers
{
    [TestFixture]
    [SetCulture("en-us")]
    public class ConfigurationsControllerTests
    {
        private const string TestUiConfigurationId = "cd87c96e-6251-489e-be85-61dc30bd61a2";
        private const string TestSiteId = "Test-Site-Id";
        private const string TestPatientId = "Test-Patient-Id";

        private Mock<ILogger<ConfigurationsController>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<ConfigurationsController>>();
        }

        [Test]
        public void GetConfigurationsTest_With_NavigationModulesUiConfiguration()
        {
            //Arrange
            var repositoryMock = SetupConfigurationRepositoryForGetConfigurationsAsync_With_NavigationModulesUiConfiguration();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();
            ActionResult<IEnumerable<UiConfiguration>> result = null;
            var controller = new ConfigurationsController(repositoryMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await controller.GetConfigurations(
                 ApplicationPart.CurrentAppointment,
                 TestSiteId,
                 TestPatientId,
                 UiConfigurationTargetAreaType.Navigation);
            act.Invoke();

            //Assert
            repositoryMock.Verify(
                repositry => repositry.GetConfigurationsAsync(
                    ApplicationPart.CurrentAppointment,
                    TestSiteId,
                    TestPatientId,
                    UiConfigurationTargetAreaType.Navigation),
                Times.Once());

            act.Should().NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult?.Value as IEnumerable<UiConfiguration>;

            contentResult.Should().NotBeNullOrEmpty();
            contentResult.First().Should().BeOfType<NavigationModulesUiConfiguration>();
            var configuration = (NavigationModulesUiConfiguration)contentResult.First();

            configuration
                .Id.Should().Be(TestUiConfigurationId);

            configuration
                .TargetAreaType.Should().Be(UiConfigurationTargetAreaType.Navigation);

            configuration
                .Tags.Should().NotBeNullOrEmpty();
            configuration
                .Tags.Should().HaveCount(2);

            configuration
                .Details.Should().NotBeNull();
            configuration
                .Details.Modules.Should().NotBeNullOrEmpty();
            configuration
                .Details.Modules.First()
                .Alias.Should().Be("ModuleOverview");
            configuration
                .Details.Modules.First()
                .Widgets.Should().NotBeNullOrEmpty();
            configuration
                .Details.Modules.First()
                .Widgets.First()
                .Alias.Should().Be("WidgetQuestions");
        }

        private static Mock<IConfigurationRepository> SetupConfigurationRepositoryForGetConfigurationsAsync_With_NavigationModulesUiConfiguration()
        {
            var repositoryMock = new Mock<IConfigurationRepository>();

            repositoryMock
                .Setup(x => x.GetConfigurationsAsync(
                    ApplicationPart.CurrentAppointment,
                    TestSiteId,
                    TestPatientId,
                    UiConfigurationTargetAreaType.Navigation))
                .Returns(Task.FromResult(new List<UiConfiguration>
                {
                    new NavigationModulesUiConfiguration
                    {
                        Id = TestUiConfigurationId,
                        TargetAreaType = UiConfigurationTargetAreaType.Navigation,
                        Tags = new[] { "tag1", "tag2" },
                        Details = new NavigationModulesDetails
                        {
                            Modules = new[]
                            {
                                new NavigationModulesItem
                                {
                                    Alias = "ModuleOverview",
                                    Widgets = new[]
                                    {
                                        new NavigationWidgetsItem
                                        {
                                            Alias = "WidgetQuestions"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }.AsEnumerable()));

            return repositoryMock;
        }
    }
}
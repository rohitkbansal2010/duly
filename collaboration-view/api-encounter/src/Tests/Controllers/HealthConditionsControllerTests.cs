// <copyright file="HealthConditionsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Controllers;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    public class HealthConditionsControllerTests
    {
        private const string PatientId = "test-patient-id";

        private Mock<ILogger<HealthConditionsController>> _loggerMock;
        private Mock<IConditionService> _serviceMock;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<HealthConditionsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public void GetHealthConditionsByPatientIdTest()
        {
            //Arrange
            ActionResult<HealthConditions> result = null;
            var controller = new HealthConditionsController(
                SetupConditionServiceForGetPatientByIdAsync(),
                _loggerMock.Object,
                _iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await controller.GetHealthConditionsByPatientId(PatientId);
            act.Invoke();

            //Assert
            _serviceMock.Verify(
                x => x.GetHealthConditionsByPatientIdAsync(PatientId),
                Times.Once());

            act.Should()
                .NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult?.Value as HealthConditions;

            contentResult.Should().NotBeNull();

            contentResult.CurrentHealthConditions.Should().NotBeNullOrEmpty();
            contentResult.CurrentHealthConditions.Should().AllBeOfType<HealthCondition>();
            contentResult.CurrentHealthConditions[0].Should().NotBeNull();
            contentResult.CurrentHealthConditions[0].Title.Should().Be("test1");

            contentResult.PreviousHealthConditions.Should().NotBeNullOrEmpty();
            contentResult.PreviousHealthConditions.Should().AllBeOfType<HealthCondition>();
            contentResult.PreviousHealthConditions[0].Should().NotBeNull();
            contentResult.PreviousHealthConditions[0].Title.Should().Be("test2");
        }

        private IConditionService SetupConditionServiceForGetPatientByIdAsync()
        {
            _serviceMock = new Mock<IConditionService>();

            _serviceMock
                .Setup(x => x.GetHealthConditionsByPatientIdAsync(PatientId))
                .Returns(Task.FromResult(new HealthConditions
                {
                    CurrentHealthConditions = new[]
                    {
                        new HealthCondition
                        {
                            Title = "test1"
                        }
                    },
                    PreviousHealthConditions = new[]
                    {
                        new HealthCondition
                        {
                            Title = "test2"
                        }
                    }
                }));

            return _serviceMock.Object;
        }
    }
}

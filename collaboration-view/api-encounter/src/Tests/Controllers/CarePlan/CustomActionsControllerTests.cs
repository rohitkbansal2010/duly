// <copyright file="CustomActionsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Controllers.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
using Duly.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers.CarePlan
{
    [TestFixture]
    [SetCulture("en-us")]
    public class CustomActionsControllerTests
    {
        private const long CustomActionId = 4;
        private static CustomActions _customActions = new CustomActions
        {
            ActionName = "Test action name",
            Description = "dummy description for test"
        };

        private Mock<ILogger<CustomActionsController>> _loggerMock;
        private Mock<ICustomActionsService> _serviceMock;

        private CustomActionsController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<CustomActionsController>>();
            _serviceMock = new Mock<ICustomActionsService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new CustomActionsController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        [Test]
        public void PostCustomActionsSuccessTest()
        {
            //Arrange
            _ = PostCustomActionsSetup();

            ActionResult<PostCustomActionsResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.PostCustomActions(_customActions);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value;
            contentResult.Should().NotBeNull();
        }

        private ICustomActionsService PostCustomActionsSetup()
        {
            _serviceMock
                .Setup(x => x.PostCustomActionsAsync(_customActions))
                .Returns(Task.FromResult(CustomActionId));

            return _serviceMock.Object;
        }
    }
}
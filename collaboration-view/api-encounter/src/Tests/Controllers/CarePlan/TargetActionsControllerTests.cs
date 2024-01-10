// <copyright file="TargetActionsControllerTests.cs" company="Duly Health and Care">
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
    public class TargetActionsControllerTests
    {
        private Mock<ILogger<TargetActionsController>> _loggerMock;
        private Mock<ITargetActionsService> _serviceMock;

        private TargetActionsController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<TargetActionsController>>();
            _serviceMock = new Mock<ITargetActionsService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new TargetActionsController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        /// <summary>
        /// 1.  should call "GetActionsForTarget" and return an Ok result with list of GetTargetActionsResponse data<see cref="GetTargetActionsSuccessTest"/>.
        /// </summary>
        [Test]
        public void GetTargetActionsSuccessTest()
        {
            //Arrange
            _ = GetTargetActionsSetup();

            ActionResult<IEnumerable<TargetActions>> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.GetActionsForTarget(1, 1);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as IEnumerable<TargetActions>;
            contentResult.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// Simulates a successful call to GetActionsForTarget.
        /// </summary>
        private IEnumerable<TargetActions> GetTargetActionsSetup()
        {
            IEnumerable<TargetActions> response = new TargetActions[]
            {
                new()
                {
                    ActionId = 1,
                    ActionName = "TestActionName",
                    Description = "TestActionDescription"
                }
            };

            _serviceMock
                .Setup(x => x.GetTargetActionsByConditionIdAndTargetIdAsync(1, 1))
                .Returns(Task.FromResult(response));

            return response;
        }
    }
}

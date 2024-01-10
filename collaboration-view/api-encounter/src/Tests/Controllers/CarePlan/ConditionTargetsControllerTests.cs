// <copyright file="ConditionTargetsControllerTests.cs" company="Duly Health and Care">
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
    public class ConditionTargetsControllerTests
    {
        private Mock<ILogger<ConditionTargetsController>> _loggerMock;
        private Mock<IConditionTargetsService> _serviceMock;

        private ConditionTargetsController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<ConditionTargetsController>>();
            _serviceMock = new Mock<IConditionTargetsService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new ConditionTargetsController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        /// <summary>
        /// 1.  should call "GetTargetsForConditions" and return an Ok result with list of GetConditionTargetsResponse data<see cref="GetTargetsForConditionsSuccessTest"/>.
        /// </summary>
        [Test]
        public void GetTargetsForConditionsSuccessTest()
        {
            //Arrange
            _ = GetTargetsForConditionsSetUp();

            ActionResult<IEnumerable<GetConditionTargetsResponse>> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.GetTargetsForConditions("text");
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as IEnumerable<GetConditionTargetsResponse>;
            contentResult.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// Simulates a successful call to GetTargetsForConditions.
        /// </summary>
        private IEnumerable<GetConditionTargetsResponse> GetTargetsForConditionsSetUp()
        {
            IEnumerable<GetConditionTargetsResponse> response = new GetConditionTargetsResponse[]
            {
                new()
                {
                }
            };

            _serviceMock
                .Setup(x => x.GetConditionTargetsByConditionId("text"))
                .Returns(Task.FromResult(response));

            return response;
        }
    }
}

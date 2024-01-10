// <copyright file="ConditionsControllerTests.cs" company="Duly Health and Care">
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
    public class ConditionsControllerTests
    {
        private Mock<ILogger<ConditionsController>> _loggerMock;
        private Mock<IConditionService> _serviceMock;

        private ConditionsController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<ConditionsController>>();
            _serviceMock = new Mock<IConditionService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new ConditionsController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        /// <summary>
        /// 1.  should call "GetConditions" and return an Ok result with list of GetConditionResponse data<see cref="GetConditionsSuccessTest"/>.
        /// </summary>
        [Test]
        public void GetConditionsSuccessTest()
        {
            //Arrange
            _ = GetConditionsSetup();

            ActionResult<IEnumerable<GetConditionResponse>> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.GetConditions();
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as IEnumerable<GetConditionResponse>;
            contentResult.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// Simulates a successful call to GetConditions.
        /// </summary>
        private IEnumerable<GetConditionResponse> GetConditionsSetup()
        {
            IEnumerable<GetConditionResponse> response = new GetConditionResponse[]
            {
                new()
                {
                    Id = 1,
                    Condition = "TestCondition"
                }
            };

            _serviceMock
                .Setup(x => x.GetConditions())
                .Returns(Task.FromResult(response));

            return response;
        }
    }
}
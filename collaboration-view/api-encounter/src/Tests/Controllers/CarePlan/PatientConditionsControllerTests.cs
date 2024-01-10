// <copyright file="PatientConditionsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Controllers.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
using Duly.Common.Infrastructure.Entities;
using Duly.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class PatientConditionsControllerTests
    {
        private Mock<ILogger<PatientConditionsController>> _loggerMock;
        private Mock<IPatientConditionsService> _serviceMock;
        private PatientConditionsController _controller;
        private int _patientPlanId = 1;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<PatientConditionsController>>();
            _serviceMock = new Mock<IPatientConditionsService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new PatientConditionsController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        /// <summary>
        /// 1.  should call "PostPatientConditions" and return result with PostPatientConditionsResponse data<see cref="PostPatientConditionsSuccessTest"/>.
        /// </summary>
        [Test]
        public void PostPatientConditionsSuccessTest()
        {
            _ = PostPatientConditionsSuccessSetup();

            ActionResult<PostPatientConditionsResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            PatientConditions patientConditions = new PatientConditions()
            {
                PatientPlanId = 3,
                AddConditionIds = new long[] { 1, 2 },
                RemoveConditionIds = new long[] { 1, 2 }
            };
            Func<Task> act = async () => result = await _controller.PostPatientConditions(patientConditions);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as PostPatientConditionsResponse;
            contentResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        /// <summary>
        /// 2.  should call "PostPatientConditions" and return Bad request result data<see cref="PostPatientConditionsNullBadRequestTest"/>.
        /// </summary>
        [Test]
        public void PostPatientConditionsNullBadRequestTest()
        {
            ActionResult<PostPatientConditionsResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            PatientConditions patientConditions = new PatientConditions();
            patientConditions = null;
            Func<Task> act = async () => result = await _controller.PostPatientConditions(patientConditions);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as BadRequestObjectResult;
            var contentResult = okResult.Value as FaultResponse;
            contentResult.Should().BeOfType<FaultResponse>();
        }

        /// <summary>
        /// 3.  should call "PostPatientConditions" and return Bad request result data<see cref="PostPatientConditionsBadRequestTest"/>.
        /// </summary>
        [Test]
        public void PostPatientConditionsBadRequestTest()
        {
            ActionResult<PostPatientConditionsResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            PatientConditions patientConditions = new PatientConditions()
            {
                PatientPlanId = 0,
                AddConditionIds = new long[] { 1, 2 },
                RemoveConditionIds = new long[] { 1, 2 }
            };
            Func<Task> act = async () => result = await _controller.PostPatientConditions(patientConditions);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as BadRequestObjectResult;
            var contentResult = okResult.Value as FaultResponse;
            contentResult.Should().BeOfType<FaultResponse>();
        }

        /// <summary>
        /// 4.  should call "GetPatientConditionByPatientPlanId" and return result with list of GetPatientConditionByPatientPlanIdResponse data<see cref="GetPatientConditionByPatientPlanIdSuccessTest"/>.
        /// </summary>
        [Test]
        public void GetPatientConditionByPatientPlanIdSuccessTest()
        {
            //Arrange
            _ = GetPatientConditionByPatientPlanIdSuccessSetup();

            ActionResult<GetPatientConditionByPatientPlanIdResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.GetPatientConditionByPatientPlanId(_patientPlanId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = (ObjectResult)result.Result;
            var contentResult = okResult.Value;
            contentResult.Should().NotBeNull();
        }

        /// <summary>
        /// 5.  should call "PostPatientConditions" and return Bad request result data<see cref="PostPatientConditionsBadRequestTest"/>.
        /// </summary>
        [Test]
        public void GetPatientConditionByPatientPlanIdBadRequestTest()
        {
            ActionResult<GetPatientConditionByPatientPlanIdResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            _patientPlanId = 0;
            Func<Task> act = async () => result = await _controller.GetPatientConditionByPatientPlanId(_patientPlanId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as BadRequestObjectResult;
            var contentResult = okResult.Value as FaultResponse;
            contentResult.Should().BeOfType<FaultResponse>();
        }

        private IEnumerable<GetPatientConditionByPatientPlanId> GetPatientConditionByPatientPlanIdSuccessSetup()
        {
            IEnumerable<GetPatientConditionByPatientPlanId> getPatientConditionByPatientPlanId = new List<GetPatientConditionByPatientPlanId>()
            {
                 new GetPatientConditionByPatientPlanId()
                 {
                     ConditionDisplayName = "TestConditionDisplayName",
                     ConditionId = 1,
                     ConditionShortName = "TestConditionShortName",
                     PatientConditionId = 2
                 }
            };

            _serviceMock
                .Setup(x => x.GetConditionByPatientPlanId(_patientPlanId))
                .Returns(Task.FromResult(getPatientConditionByPatientPlanId));

            return getPatientConditionByPatientPlanId;
        }

        private IEnumerable<long> PostPatientConditionsSuccessSetup()
        {
            IEnumerable<long> res = new List<long>() { 1, 2 };

            PatientConditions patientConditions = new PatientConditions()
            {
                PatientPlanId = 3,
                AddConditionIds = new long[] { 1, 2 },
                RemoveConditionIds = new long[] { 1, 2 }
            };

            _serviceMock
                .Setup(x => x.PostPatientConditionsAsync(patientConditions))
                .Returns(Task.FromResult(res));

            return res;
        }
    }
}
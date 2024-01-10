// <copyright file="PatientTargetsControllerTests.cs" company="Duly Health and Care">
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
    public class PatientTargetsControllerTests
    {
        private Mock<ILogger<PatientTargetsController>> _loggerMock;
        private Mock<IPatientTargetsService> _serviceMock;
        private PatientTargetsController _controller;
        private int _patientTargetId = 1;
        private long _patientTargetID = 1;

        private PatientTargets _patientTarget = new PatientTargets()
        {
            TargetId = 1,
            ConditionId = 1,
            MaxValue = "TestMaxValue",
            MeasurementUnit = "TestMinValue",
            MinValue = "TestMinValue",
            PatientPlanId = 1,
        };

        private UpdatePatientTargetReviewStatus _updatePatientTargetReviewStatus = new UpdatePatientTargetReviewStatus()
        {
            IsReviewed = true
        };

        private UpdatePatientTargets _updatePatientTargets = new UpdatePatientTargets()
        {
            TargetId = 1,
            BaseValue = "1",
            ConditionIds = new List<long>() { 1, 5 },
            MaxValue = "TestMaxValue",
            MeasurementUnit = "TestMinValue",
            MinValue = "TestMinValue",
            PatientPlanId = 1,
            RecentValue = "1",
            TargetValue = "5"
        };

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<PatientTargetsController>>();
            _serviceMock = new Mock<IPatientTargetsService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new PatientTargetsController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        /// <summary>
        /// 1.  should call "GetPatientTargetsByPatientPlanId" and return an Ok result with list of GetPatientTargets data<see cref="GetPatientTargetsSuccessTest"/>.
        /// </summary>
        [Test]
        public void GetPatientTargetsByPatientPlanIdSuccessTest()
        {
            //Arrange
            _ = GetPatientTargetsByPatientPlanIdSuccessSetup();

            ActionResult<IEnumerable<GetPatientTargets>> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.GetPatientTargetsByPatientPlanId(1);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as IEnumerable<GetPatientTargets>;
            contentResult.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// 2.  should call "PostPatientPlan" and return Bad request result data<see cref="PostPatientPlanBadRequestTest"/>.
        /// </summary>
        [Test]
        public void GetPatientTargetsByPatientPlanIdBadRequestTest()
        {
            ActionResult<IEnumerable<GetPatientTargets>> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.GetPatientTargetsByPatientPlanId(0);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as BadRequestObjectResult;
            var contentResult = okResult.Value as FaultResponse;
            contentResult.Should().BeOfType<FaultResponse>();
        }

        /// <summary>
        /// 3.  should call "PostPatientTargets" and return Bad request result data<see cref="PostPatientTargetsBadRequestTest"/>.
        /// </summary>
        [Test]
        public void PostPatientTargetsBadRequestTest()
        {
            ActionResult<PostPatientTargetResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            PatientTargets patientTarget = new PatientTargets();
            patientTarget = null;

            Func<Task> act = async () => result = await _controller.PostPatientTargets(patientTarget);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as BadRequestObjectResult;
            var contentResult = okResult.Value as FaultResponse;
            contentResult.Should().BeOfType<FaultResponse>();
        }

        /// <summary>
        /// 4.  should call "PostPatientTargets" and return an Ok result with PostPatientTargetResponse data<see cref="PostPatientTargetsSuccessTest"/>.
        /// </summary>
        [Test]
        public void PostPatientTargetsSuccessTest()
        {
            //Arrange
            _ = PostPatientTargetsSuccessSetup();

            ActionResult<PostPatientTargetResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.PostPatientTargets(_patientTarget);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as PostPatientTargetResponse;
            contentResult.StatusCode.Should().Be(StatusCodes.Status201Created.ToString());
        }

        /// <summary>
        /// 5.  should call "PostPatientTargets" and return result with data Status409Conflict data<see cref="PostPatientTargets409ConflictTest"/>.
        /// </summary>
        [Test]
        public void PostPatientTargets409ConflictTest()
        {
            //Arrange
            _ = PostPatientPlanStatus409ConflictSetup();

            ActionResult<PostPatientTargetResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.PostPatientTargets(_patientTarget);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as PostPatientTargetResponse;
            contentResult.StatusCode.Should().Equals(StatusCodes.Status409Conflict.ToString());
        }

        /// <summary>
        /// 6.  should call "PostPatientTargets" and return result with Status400BadRequest data<see cref="PostPatientTargetsStatus400BadRequestTest"/>.
        /// </summary>
        [Test]
        public void PostPatientTargetsStatus400BadRequestTest()
        {
            //Arrange
            _ = PostPatientPlanStatus400BadRequestSetup();

            ActionResult<PostPatientTargetResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.PostPatientTargets(_patientTarget);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as PostPatientTargetResponse;
            contentResult.StatusCode.Should().Equals(StatusCodes.Status400BadRequest.ToString());
        }

        /// <summary>
        /// 7.  should call "DeletePatientTarget" and return result with Status400BadRequest data<see cref="PostPatientTargetsStatus400BadRequestTest"/>.
        /// </summary>
        [Test]
        public void DeletePatientTargetBadRequestTest()
        {
            //Arrange
            ActionResult<DeletePatientTargetResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            int patientTargetId = 0;
            Func<Task> act = async () => result = await _controller.DeletePatientTarget(patientTargetId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as BadRequestObjectResult;
            var contentResult = okResult.Value as FaultResponse;
            contentResult.Should().BeOfType<FaultResponse>();
        }

        /// <summary>
        /// 8.  should call "DeletePatientTarget" and return result with Status400BadRequest data<see cref="PostPatientTargetsStatus400BadRequestTest"/>.
        /// </summary>
        [Test]
        public void DeletePatientTargetSuccessTest()
        {
            //Arrange
            _ = DeletePatientTargetSuccessSetup();

            ActionResult<DeletePatientTargetResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            int patientTargetId = 1;
            Func<Task> act = async () => result = await _controller.DeletePatientTarget(patientTargetId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as DeletePatientTargetResponse;
            contentResult.IsDeleted.Should().Be(true);
            contentResult.StatusCode = StatusCodes.Status200OK.ToString();
        }

        /// <summary>
        /// 9.  should call "UpdatePatientTargetReviewStatus" and return result with UpdatePatientTargetReviewStatusResponse data<see cref="UpdatePatientTargetReviewStatusSuccessTest"/>.
        /// </summary>
        [Test]
        public void UpdatePatientTargetReviewStatusSccessTest()
        {
            //Arrange
            _ = UpdatePatientTargetReviewStatusSccessSetup();

            ActionResult<UpdatePatientTargetReviewStatusResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.UpdatePatientTargetReviewStatus(_updatePatientTargetReviewStatus, _patientTargetId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as UpdatePatientTargetReviewStatusResponse;
            contentResult.StatusCode = StatusCodes.Status200OK.ToString();
        }

        /// <summary>
        /// 10.  should call "UpdatePatientTargets" and return result with UpdatePatientTargets data<see cref="UpdatePatientTargetsBadRequestTest"/>.
        /// </summary>
        [Test]
        public void UpdatePatientTargetsBadRequestTest()
        {
            //Arrange
            ActionResult<UpdatePatientTargets> result = null;
            _controller.MockObjectValidator();

            //Act
            UpdatePatientTargets updatePatientTargets = new UpdatePatientTargets();
            updatePatientTargets = null;
            _patientTargetID = 0;
            Func<Task> act = async () => result = await _controller.UpdatePatientTargets(updatePatientTargets, _patientTargetID);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as BadRequestObjectResult;
            var contentResult = okResult.Value as FaultResponse;
            contentResult.Should().BeOfType<FaultResponse>();
        }

        /// <summary>
        /// 11.  should call "UpdatePatientTargets" and return result with Status400BadRequest data<see cref="UpdatePatientTargets400BadRequestTest"/>.
        /// </summary>
        [Test]
        public void UpdatePatientTargets400BadRequestTest()
        {
            //Arrange
            _ = UpdatePatientTargets400BadRequestSetup();

            ActionResult<UpdatePatientTargets> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.UpdatePatientTargets(_updatePatientTargets, _patientTargetId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as UpdatePatientTargetsResponse;
            contentResult.StatusCode.Should().Equals(StatusCodes.Status400BadRequest.ToString());
        }

        /// <summary>
        /// 13.  should call "UpdatePatientTargets" and return result with UpdatePatientTargets data<see cref="UpdatePatientTargetsSuccessTest"/>.
        /// </summary>
        [Test]
        public void UpdatePatientTargetsSuccessTest()
        {
            //Arrange
            _ = UpdatePatientTargetsSuccessTestSetup();

            ActionResult<UpdatePatientTargets> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.UpdatePatientTargets(_updatePatientTargets, _patientTargetId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as UpdatePatientTargetsResponse;
            contentResult.PatientTargetId.Should().Be(1);
            contentResult.StatusCode.Should().Equals(StatusCodes.Status202Accepted.ToString());
        }

        /// <summary>
        /// Simulates a successful call to GetPatientTargetsByPatientPlanId.
        /// </summary>
        private IEnumerable<GetPatientTargets> GetPatientTargetsByPatientPlanIdSuccessSetup()
        {
            IEnumerable<GetPatientTargets> response = new GetPatientTargets[]
            {
                new()
                {
                   TargetId = 1,
                   TargetName = "TestTargetName",
                   MinValue = "TestMinValue",
                   MaxValue = "TestMaxValue",
                   MeasurementUnit = "TestMeasurementUnit",
                   PatientConditionId = "TestPatientConditionId",
                   ConditionId = 1,
                   IsReviewed = 1
                }
            };

            _serviceMock
                .Setup(x => x.GetPatientTargetsByPatientPlanIdAsync(1))
                .Returns(Task.FromResult(response));

            return response;
        }

        private long PostPatientTargetsSuccessSetup()
        {
            _serviceMock
                .Setup(x => x.PostPatientTargetsAsync(_patientTarget))
                .Returns(Task.FromResult<long>(1));

            return 1;
        }

        private long PostPatientPlanStatus409ConflictSetup()
        {
            _serviceMock
                .Setup(x => x.PostPatientTargetsAsync(_patientTarget))
                .Returns(Task.FromResult<long>(0));

            return 0;
        }

        private long PostPatientPlanStatus400BadRequestSetup()
        {
            _serviceMock
                .Setup(x => x.PostPatientTargetsAsync(_patientTarget))
                .Returns(Task.FromResult<long>(-1));

            return -1;
        }

        private int DeletePatientTargetSuccessSetup()
        {
            _serviceMock
                .Setup(x => x.DeletePatientTargetAsync(_patientTargetId))
                .Returns(Task.FromResult(1));

            return 1;
        }

        private long UpdatePatientTargetReviewStatusSccessSetup()
        {
            _serviceMock
                .Setup(x => x.UpdatePatientTargetReviewStatusAsync(_updatePatientTargetReviewStatus, _patientTargetId))
                .Returns(Task.FromResult<long>(1));

            return 1;
        }

        private long UpdatePatientTargets400BadRequestSetup()
        {
            _serviceMock
                .Setup(x => x.UpdatePatientTargetsAsync(_updatePatientTargets, _patientTargetId))
                .Returns(Task.FromResult<long>(-1));

            return -1;
        }

        private long UpdatePatientTargetsSuccessTestSetup()
        {
            _serviceMock
                .Setup(x => x.UpdatePatientTargetsAsync(_updatePatientTargets, _patientTargetId))
                .Returns(Task.FromResult<long>(1));

            return 1;
        }
    }
}
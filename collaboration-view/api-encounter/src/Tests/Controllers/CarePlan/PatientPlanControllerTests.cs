// <copyright file="PatientPlanControllerTests.cs" company="Duly Health and Care">
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
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers.CarePlan
{
    [TestFixture]
    [SetCulture("en-us")]
    public class PatientPlanControllerTests
    {
        private Mock<ILogger<PatientPlanController>> _loggerMock;
        private Mock<IPatientPlanService> _serviceMock;
        private PatientPlanController _controller;
        private PatientPlan _patientPlanNullRequest = new PatientPlan();
        private PatientPlan _patientPlan = new PatientPlan()
        {
            AppointmentId = "TestAppointmentId",
            PatientId = "TestPatientId",
            ProviderId = "TestProviderId",
            FlourishingStatement = "TestFlourishingStatement"
        };

        private UpdateFlourishStatementRequest _updateFlourishStatementRequest = new UpdateFlourishStatementRequest()
        {
            FlourishStatement = "TestFlourishStatement",
            PatientPlanId = 999
        };

        private int _id = 0;
        private string _patientId = "TestPatientId";

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<PatientPlanController>>();
            _serviceMock = new Mock<IPatientPlanService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new PatientPlanController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        /// <summary>
        /// 1.  should call "PostPatientPlan" and return Bad request result data<see cref="PostPatientPlanBadRequestTest"/>.
        /// </summary>
        [Test]
        public void PostPatientPlanBadRequestTest()
        {
            ActionResult<PostPatientPlanResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            _patientPlanNullRequest = null;
            Func<Task> act = async () => result = await _controller.PostPatientPlan(_patientPlanNullRequest);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as BadRequestObjectResult;
            var contentResult = okResult.Value as FaultResponse;
            contentResult.Should().BeOfType<FaultResponse>();
        }

        /// <summary>
        /// 2.  should call "PostPatientPlan" and return success request result data<see cref="GetConditionsSuccessTest"/>.
        /// </summary>
        [Test]
        public void PostPatientPlanSuccessTest()
        {
            //Arrange
            _ = PostPatientPlanSuccessSetup();

            ActionResult<PostPatientPlanResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.PostPatientPlan(_patientPlan);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value;
            contentResult.Should().NotBeNull();
            contentResult.Should().BeOfType<PostPatientPlanResponse>();
        }

        /// <summary>
        /// 3.  should call "PostPatientPlan" and return result with data Status409Conflict data<see cref="PostPatientPlanStatus409ConflictTest"/>.
        /// </summary>
        [Test]
        public void PostPatientPlanStatus409ConflictTest()
        {
            //Arrange
            _ = PostPatientPlanStatus409ConflictSetup();

            ActionResult<PostPatientPlanResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.PostPatientPlan(_patientPlan);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as PostPatientPlanResponse;
            contentResult.PatientPlanId.Should().Be(0);
            contentResult.StatusCode.Should().Equals(StatusCodes.Status409Conflict);
        }

        /// <summary>
        /// 4.  should call "UpdatePatientPlanStatusById" and return Bad Request with FaultResponse data<see cref="UpdatePatientPlanStatusByIdBadRequestTest"/>.
        /// </summary>
        [Test]
        public void UpdatePatientPlanStatusByIdBadRequestTest()
        {
            ActionResult<UpdatePatientPlanStatusByIdResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.UpdatePatientPlanStatusById(_id);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as BadRequestObjectResult;
            var contentResult = okResult.Value as FaultResponse;
            contentResult.Should().BeOfType<FaultResponse>();
        }

        /// <summary>
        /// 5.  should call "UpdatePatientPlanStatusById" and return success request result with UpdatePatientPlanStatusByIdResponse data<see cref="UpdatePatientPlanStatusByIdSuccessTest"/>.
        /// </summary>
        [Test]
        public void UpdatePatientPlanStatusByIdSuccessTest()
        {
            //Arrange
            _ = UpdatePatientPlanStatusByIdSuccessSetup();

            ActionResult<UpdatePatientPlanStatusByIdResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            _id = 1;
            Func<Task> act = async () => result = await _controller.UpdatePatientPlanStatusById(_id);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as UpdatePatientPlanStatusByIdResponse;
            contentResult.Should().NotBeNull();
            contentResult.StatusCode.Should().Equals(StatusCodes.Status200OK);
        }

        /// <summary>
        /// 6.  should call "GetPatientPlanByPatientId" and return Bad Request with FaultResponse data<see cref="UpdatePatientPlanStatusByIdBadRequestTest"/>.
        /// </summary>
        [Test]
        public void GetPatientPlanByPatientIdBadRequestTest()
        {
            ActionResult<GetPatientPlanByIdResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            _patientId = null;
            Func<Task> act = async () => result = await _controller.GetPatientPlanByPatientId(_patientId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as BadRequestObjectResult;
            var contentResult = okResult.Value as FaultResponse;
            contentResult.Should().BeOfType<FaultResponse>();
        }

        /// <summary>
        /// 7.  should call "GetPatientPlanByPatientId" and return an Ok result with GetPatientPlanByIdResponse data<see cref="GetPatientPlanByPatientIdSuccessTest"/>.
        /// </summary>
        [Test]
        public void GetPatientPlanByPatientIdSuccessTest()
        {
            //Arrange
            _ = GetPatientPlanByPatientIdSuccessSetup();

            ActionResult<GetPatientPlanByIdResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.GetPatientPlanByPatientId(_patientId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = (ObjectResult)result.Result;
            var contentResult = okResult.Value;
            contentResult.Should().NotBeNull();
        }

        /// UpdateFlourishStatement
        /// <summary>
        /// 8.  should call "UpdateFlourishStatement" and return an Ok result with UpdateFlourishStatementResponse data<see cref="UpdateFlourishStatementSuccessTest"/>.
        /// </summary>
        [Test]
        public void UpdateFlourishStatementSuccessTest()
        {
            //Arrange
            _ = UpdateFlourishStatementSuccessSetup(_updateFlourishStatementRequest);

            ActionResult<UpdateFlourishStatementResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.UpdateFlourishStatement(_updateFlourishStatementRequest);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = (ObjectResult)result.Result;
            var contentResult = okResult.Value;
            contentResult.Should().NotBeNull();
        }

        /// <summary>
        /// 9.  should call "UpdateFlourishStatement" and return Bad Request with FaultResponse data<see cref="UpdatePatientPlanStatusByIdBadRequestTest"/>.
        /// </summary>
        [Test]
        public void UpdateFlourishStatementBadRequestTest()
        {
            ActionResult<UpdateFlourishStatementResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            _updateFlourishStatementRequest = null;
            Func<Task> act = async () => result = await _controller.UpdateFlourishStatement(_updateFlourishStatementRequest);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as BadRequestObjectResult;
            var contentResult = okResult.Value as FaultResponse;
            contentResult.Should().BeOfType<FaultResponse>();
        }

        private int PostPatientPlanSuccessSetup()
        {
            _serviceMock
                .Setup(x => x.PostPatientPlanAsync(_patientPlan))
                .Returns(Task.FromResult(1));

            return 1;
        }

        private int PostPatientPlanStatus409ConflictSetup()
        {
            _serviceMock
                .Setup(x => x.PostPatientPlanAsync(_patientPlan))
                .Returns(Task.FromResult(0));

            return 0;
        }

        private bool UpdatePatientPlanStatusByIdSuccessSetup()
        {
            _id = 1;
            _serviceMock
                .Setup(x => x.UpdatePatientPlanStatusByIdAsync(_id))
                .Returns(Task.FromResult(true));

            return true;
        }

        private GetPatientPlanByPatientId GetPatientPlanByPatientIdSuccessSetup()
        {
            GetPatientPlanByPatientId patientPlanByPatientId = new GetPatientPlanByPatientId()
            {
                FlourishStatement = "TestFlourishStatement",
                PatientPlanId = 99,
                PlanName = "TestPlanName"
            };

            _serviceMock
                .Setup(x => x.GetPatientPlanByPatientIdAsync(_patientId))
                .Returns(Task.FromResult(patientPlanByPatientId));

            return patientPlanByPatientId;
        }

        private long UpdateFlourishStatementSuccessSetup(UpdateFlourishStatementRequest request)
        {
            _serviceMock
                .Setup(x => x.UpdateFlourishStatementAsync(request))
                .Returns(Task.FromResult<long>(1));

            return 1;
        }
    }
}
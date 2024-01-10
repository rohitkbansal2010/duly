// <copyright file="PatientLifeGoalControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Controllers.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces.CarePlan;
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
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers.CarePlan
{
    [TestFixture]
    [SetCulture("en-us")]
    public class PatientLifeGoalControllerTests
    {
        private Mock<ILogger<PatientLifeGoalController>> _loggerMock;
        private Mock<IPatientLifeGoalService> _serviceMock;
        private PatientLifeGoalController _controller;
        private List<long> _patientLifeGoalIds = new List<long>() { 1, 2 };
        private List<long> _patientLifeGoalIdsForMapping = new List<long>() { 99 };
        private long _patientTargetIdForMapping =  98;

        private PostRequestForLifeGoals _postRequestForLifeGoals = new PostRequestForLifeGoals()
        {
            PatientPlanId = 99,
            PatientLifeGoal = new List<PatientLifeGoal>()
            {
                new PatientLifeGoal()
                {
                    CategoryName = "TestCategoryName",
                    LifeGoalDescription = "TestLifeGoalDescription",
                    LifeGoalName = "TestLifeGoalName",
                    PatientLifeGoalId = 99
                }
            },
            DeletedLifeGoalIds = new List<long>() { 2, 3 }
        };

        private PostOrUpdatePatientLifeGoalResponse _postOrUpdatePatientLifeGoalResponse = new PostOrUpdatePatientLifeGoalResponse()
        {
            DeletedLifeGoalIds = "1,2,3",
            Message = "TestMessage",
            PatientLifeGoals = new List<PatientLifeGoal>()
            {
                new PatientLifeGoal()
                {
                    CategoryName = "TestCategoryName",
                    LifeGoalDescription = "TestLifeGoalDescription",
                    LifeGoalName = "TestLifeGoalName",
                    PatientLifeGoalId = 99
                }
            },
            StatusCode = StatusCodes.Status202Accepted.ToString()
        };

        private IEnumerable<GetPatientLifeGoalByPatientPlanId> _getPatientLifeGoalByPatientPlanIdResponse = new List<GetPatientLifeGoalByPatientPlanId>()
        {
               new GetPatientLifeGoalByPatientPlanId()
                {
                   CategoryName = "TestCategoryName",
                   LifeGoalDescription = "TestLifeGoalDescription",
                   LifeGoalName = "TestLifeGoalName",
                   PatientLifeGoalId = 99
                }
        };

        private IEnumerable<PatientLifeGoalTargetMapping> _patientLifeGoalTargetMappings = new List<PatientLifeGoalTargetMapping>()
        {
            new PatientLifeGoalTargetMapping()
            {
                 PatientLifeGoalId = 99,
                 PatientTargetId = 98
            }
        };

        private IEnumerable<GetPatientLifeGoalTargetMapping> _getPatientLifeGoalTargetMapping = new List<GetPatientLifeGoalTargetMapping>()
        {
            new GetPatientLifeGoalTargetMapping()
            {
                 PatientLifeGoalId = 99,
                 LifeGoalDescription = "TestLifeGoalDescription",
                 LifeGoalName = "TestLifeGoalName"
            }
        };

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<PatientLifeGoalController>>();
            _serviceMock = new Mock<IPatientLifeGoalService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new PatientLifeGoalController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        /// <summary>
        /// 1.  should call "PostOrUpdatePatientLifeGoal" and return success result with PostOrUpdatePatientLifeGoalResponse data<see cref="PostOrUpdatePatientLifeGoalSuccessTest"/>.
        /// </summary>
        [Test]
        public void PostOrUpdatePatientLifeGoalSuccessTest()
        {
            //Arrange
            _ = PostOrUpdatePatientLifeGoalSuccessSetup();

            ActionResult<PostOrUpdatePatientLifeGoalResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.PostOrUpdatePatientLifeGoal(_postRequestForLifeGoals);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value;
            contentResult.Should().NotBeNull();
            contentResult.Should().BeOfType<PostOrUpdatePatientLifeGoalResponse>();
        }

        /// <summary>
        /// 2.  should call "DeletePatientLifeGoal" and return success result with list of DeletePatientLifeGoalResponse data<see cref="DeletePatientLifeGoalSuccessTest"/>.
        /// </summary>
        [Test]
        public void DeletePatientLifeGoalSuccessTest()
        {
            //Arrange
            _ = DeletePatientLifeGoalAsyncSuccessSetup();

            ActionResult<IEnumerable<DeletePatientLifeGoalResponse>> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.DeletePatientLifeGoal(_patientLifeGoalIds);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as List<DeletePatientLifeGoalResponse>;
            contentResult.Should().NotBeNull();
            contentResult.Should().BeOfType<List<DeletePatientLifeGoalResponse>>();
            contentResult.FirstOrDefault().StatusCode.Should().Be(StatusCodes.Status200OK.ToString());
        }

        /// <summary>
        /// 3.  should call "DeletePatientLifeGoal" and return success result with list of DeletePatientLifeGoalResponse data<see cref="DeletePatientLifeGoalNotFoundTest"/>.
        /// </summary>
        [Test]
        public void DeletePatientLifeGoalNotFoundTest()
        {
            //Arrange
            _ = DeletePatientLifeGoalAsyncNotFoundSetup();

            ActionResult<IEnumerable<DeletePatientLifeGoalResponse>> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.DeletePatientLifeGoal(_patientLifeGoalIds);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as List<DeletePatientLifeGoalResponse>;
            contentResult.Should().NotBeNull();
            contentResult.Should().BeOfType<List<DeletePatientLifeGoalResponse>>();
            contentResult.FirstOrDefault().StatusCode.Should().Be(StatusCodes.Status204NoContent.ToString());
        }

        /// <summary>
        /// 4.  should call "GetPatientLifeGoalById" and return success result with GetPatientLifeGoalByPatientPlanIdResponse data<see cref="GetPatientLifeGoalByIdSuccessTest"/>.
        /// </summary>
        [Test]
        public void GetPatientLifeGoalByIdSuccessTest()
        {
            //Arrange
            _ = GetPatientLifeGoalByIdSuccessSetup();

            ActionResult<GetPatientLifeGoalByPatientPlanIdResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            long patientPlanId = 1;
            Func<Task> act = async () => result = await _controller.GetPatientLifeGoalById(patientPlanId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as GetPatientLifeGoalByPatientPlanIdResponse;
            contentResult.Should().NotBeNull();
            contentResult.Should().BeOfType<GetPatientLifeGoalByPatientPlanIdResponse>();
            contentResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        /// <summary>
        /// 5.  should call "PostPatientLifeGoalTargetMapping" and return success result with GetPatientLifeGoalByPatientPlanIdResponse data<see cref="PostPatientLifeGoalTargetMappingSuccessTest"/>.
        /// </summary>
        [Test]
        public void PostPatientLifeGoalTargetMappingSuccessTest()
        {
            //Arrange
            _ = PostPatientLifeGoalTargetMappingSetup();

            ActionResult<PostPatientLifeGoalTargetMappingResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.PostPatientLifeGoalTargetMapping(_patientTargetIdForMapping, _patientLifeGoalIdsForMapping);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as PostPatientLifeGoalTargetMappingResponse;
            contentResult.Should().NotBeNull();
            contentResult.Should().BeOfType<PostPatientLifeGoalTargetMappingResponse>();
            contentResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        /// <summary>
        /// 5.  should call "GetPatientLifeGoalTargetMappingByPatientTargetId" and return success result with GetPatientLifeGoalByPatientPlanIdResponse data<see cref="GetPatientLifeGoalTargetMappingByPatientTargetIdSuccessTest"/>.
        /// </summary>
        [Test]
        public void GetPatientLifeGoalTargetMappingByPatientTargetIdSuccessTest()
        {
            //Arrange
            _ = GetPatientLifeGoalTargetMappingByPatientTargetIdSuccessSetup();

            ActionResult<IEnumerable<GetPatientLifeGoalTargetMapping>> result = null;
            _controller.MockObjectValidator();

            //Act
            long patientTargetId = 1;
            Func<Task> act = async () => result = await _controller.GetPatientLifeGoalTargetMappingByPatientTargetId(patientTargetId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value as List<GetPatientLifeGoalTargetMapping>;
            contentResult.Should().NotBeNull();
            contentResult.Should().BeOfType<List<GetPatientLifeGoalTargetMapping>>();
        }

        private PostOrUpdatePatientLifeGoalResponse PostOrUpdatePatientLifeGoalSuccessSetup()
        {
            _serviceMock
                .Setup(x => x.PostOrUpdateLifeGoalAsync(_postRequestForLifeGoals))
                .Returns(Task.FromResult(_postOrUpdatePatientLifeGoalResponse));

            return _postOrUpdatePatientLifeGoalResponse;
        }

        private long DeletePatientLifeGoalAsyncSuccessSetup()
        {
            _serviceMock
              .Setup(x => x.DeletePatientLifeGoalAsync(1))
              .Returns(Task.FromResult<long>(1));

            _serviceMock
              .Setup(x => x.DeletePatientLifeGoalAsync(2))
              .Returns(Task.FromResult<long>(2));

            return 1;
        }

        private long DeletePatientLifeGoalAsyncNotFoundSetup()
        {
            _serviceMock
              .Setup(x => x.DeletePatientLifeGoalAsync(1))
              .Returns(Task.FromResult<long>(0));

            return 0;
        }

        private IEnumerable<GetPatientLifeGoalByPatientPlanId> GetPatientLifeGoalByIdSuccessSetup()
        {
            _serviceMock
              .Setup(x => x.GetPatientLifeGoalByPatientPlanIdAsync(1))
              .Returns(Task.FromResult(_getPatientLifeGoalByPatientPlanIdResponse));

            return _getPatientLifeGoalByPatientPlanIdResponse;
        }

        private long PostPatientLifeGoalTargetMappingSetup()
        {
            long patientTargetId = 5;
            List<long> patientLifeGoalIds = new List<long>() { 99 };

            _serviceMock
             .Setup(x => x.PostPatientLifeGoalTargetMappingAsync(patientTargetId, patientLifeGoalIds))
             .Returns(Task.FromResult<long>(1));

            return 1;
        }

        private IEnumerable<GetPatientLifeGoalTargetMapping> GetPatientLifeGoalTargetMappingByPatientTargetIdSuccessSetup()
        {
            _serviceMock
             .Setup(x => x.GetPatientLifeGoalTargetMappingByPatientIdAsync(1))
             .Returns(Task.FromResult(_getPatientLifeGoalTargetMapping));

            return _getPatientLifeGoalTargetMapping;
        }
    }
}
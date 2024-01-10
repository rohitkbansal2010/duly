// <copyright file="PatientActionsControllerTests.cs" company="Duly Health and Care">
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
    public class PatientActionsControllerTests
    {
        private const long PatientTargetId = 4;
        private const long PatientActionId = 7;
        private const long ResponseId = 1;
        private static UpdateActionProgress updateActionProgress = new UpdateActionProgress
        {
            Progress = 5,
            Notes = "test notes"
        };
        private static IEnumerable<PatientActions> _patientActions = new PatientActions[]
       {
            new()
            {
               PatientTargetId = 2,
               ActionId = 1,
               CustomActionId = 8,
               Deleted = false
            }
       };
        private Mock<ILogger<PatientActionsController>> _loggerMock;
        private Mock<IPatientActionsService> _serviceMock;

        private PatientActionsController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<PatientActionsController>>();
            _serviceMock = new Mock<IPatientActionsService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new PatientActionsController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        /// <summary>
        /// 1.  should call "GetPatientActionsByPatientTargetId" and return an Ok result with list of PatientActions data<see cref="GetConditionsSuccessTest"/>.
        /// </summary>
        [Test]
        public void GetPatientActionsByPatientTargetIdSuccessTest()
        {
            //Arrange
            _ = GetPatientActionsByPatientTargetIdSetup();

            ActionResult<IEnumerable<GetPatientActions>> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.GetPatientActionsByPatientTargetId(1);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as IEnumerable<GetPatientActions>;
            contentResult.Should().NotBeNullOrEmpty();
        }

        public void PostPatientActionsSuccessTest ()
        {
            //Arrange
            _ = PostPatientActionsSetup();

            ActionResult<PostPatientActionsResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.PostPatientActions(_patientActions);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value;
            contentResult.Should().NotBeNull();
            contentResult.Should().BeOfType<PostPatientActionsResponse>();
        }

        public void UpdateActionProgressSuccessTest()
        {
            //Arrange
            _ = UpdateActionProgressSetup();

            ActionResult<UpdateActionProgressResponse> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.UpdateActionProgress(updateActionProgress, PatientActionId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as ObjectResult;
            var contentResult = okResult.Value;
            contentResult.Should().NotBeNull();
            contentResult.Should().BeOfType<PostPatientActionsResponse>();
        }

        /// <summary>
        /// Simulates a successful call to GetPatientActionsByPatientTargetId.
        /// </summary>
        private IEnumerable<GetPatientActions> GetPatientActionsByPatientTargetIdSetup()
        {
            IEnumerable<GetPatientActions> response = new GetPatientActions[]
            {
                new()
                {
                    PatientActionId = 1,
                    ActionId = 1,
                    ActionName = "TestActionName",
                    Description = "TestActionDescription"
                }
            };
            _serviceMock
                .Setup(x => x.GetPatientActionsByPatientTargetIdAsync(1))
                .Returns(Task.FromResult(response));

            return response;
        }

        private IPatientActionsService PostPatientActionsSetup()
        {
            _serviceMock
                .Setup(x => x.PostPatientActionsAsync(_patientActions))
                .Returns(Task.FromResult(PatientTargetId));

            return _serviceMock.Object;
        }

        private IPatientActionsService UpdateActionProgressSetup()
        {
            _serviceMock
                .Setup(x => x.UpdateActionProgressAsync(updateActionProgress, PatientActionId))
                .Returns(Task.FromResult(ResponseId));

            return _serviceMock.Object;
        }
    }
}

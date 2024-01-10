// <copyright file="AllergiesControllerTests.cs" company="Duly Health and Care">
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    [SetCulture("en-us")]
    public class AllergiesControllerTests
    {
        private const string TestPatientId = "test-patient-id";

        private Mock<ILogger<AllergiesController>> _loggerMock;
        private Mock<IAllergyService> _serviceMock;

        private AllergiesController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<AllergiesController>>();
            _serviceMock = new Mock<IAllergyService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new AllergiesController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        [Test]
        public void GetAllergiesTest()
        {
            //Arrange
            var allergies = SetupService();

            ActionResult<IEnumerable<Allergy>> result = null;
            _controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await _controller.GetAllergiesForSpecificPatient(TestPatientId);
            act.Invoke();

            //Assert
            act.Should().NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as IEnumerable<Allergy>;
            contentResult.Should().NotBeNullOrEmpty();
            contentResult.First().Id.Should().Be(TestPatientId);
        }

        private IEnumerable<Allergy> SetupService()
        {
            IEnumerable<Allergy> allergies = new Allergy[]
            {
                new()
                {
                    Id = TestPatientId
                }
            };

            _serviceMock
                .Setup(x => x.GetAllergiesForPatientAsync(TestPatientId))
                .Returns(Task.FromResult(allergies));

            return allergies;
        }
    }
}
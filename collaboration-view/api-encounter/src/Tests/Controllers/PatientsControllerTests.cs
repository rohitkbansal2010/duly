// <copyright file="PatientsControllerTests.cs" company="Duly Health and Care">
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
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    [SetCulture("en-us")]
    public class PatientsControllerTests
    {
        private const string PatientId = "test-patient-id";

        private Mock<ILogger<PatientsController>> _loggerMock;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<PatientsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public void GetPatientByIdTest()
        {
            //Arrange
            ActionResult<Patient> result = null;
            var controller = new PatientsController(
                SetupPatientServiceForGetPatientByIdAsync(),
                _loggerMock.Object,
                _iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await controller.GetPatientById(PatientId);
            act.Invoke();

            //Assert
            act.Should()
                .NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult?.Value as Patient;

            contentResult.Should().NotBeNull();
            contentResult.GeneralInfo.Should().NotBeNull();
            contentResult.GeneralInfo.Id.Should().Be(PatientId);
            contentResult.BirthDate.Should().Be(new DateTime(1970, 7, 15));
            contentResult.Gender.Should().Be(Gender.Female);
        }

        private static IPatientService SetupPatientServiceForGetPatientByIdAsync()
        {
            var serviceMock = new Mock<IPatientService>();

            serviceMock
                .Setup(x => x.GetPatientByIdAsync(PatientId))
                .Returns(Task.FromResult(new Patient
                {
                    GeneralInfo = new PatientGeneralInfo
                    {
                        Id = PatientId,
                        HumanName = new HumanName
                        {
                            FamilyName = "Reyes",
                            GivenNames = new[] { "Ana", "Maria" }
                        }
                    },
                    BirthDate = new DateTime(1970, 7, 15),
                    Gender = Gender.Female
                }));

            return serviceMock.Object;
        }
    }
}
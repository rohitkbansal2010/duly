// <copyright file="MedicationsControllerTests.cs" company="Duly Health and Care">
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
    public class MedicationsControllerTests
    {
        private const string PatientId = "test-patient-id";

        private Mock<ILogger<MedicationsController>> _loggerMock;
        private Mock<IMedicationService> _serviceMock;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<MedicationsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public void GetMedicationsByPatientIdTest()
        {
            //Arrange
            ActionResult<Medications> result = null;
            var controller = new MedicationsController(
                SetupMedicationServiceForGetPatientByIdAsync(),
                _loggerMock.Object,
                _iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            //Act
            Func<Task> act = async () => result = await controller.GetMedicationsByPatientId(PatientId);
            act.Invoke();

            //Assert
            _serviceMock.Verify(
                x => x.GetMedicationsByPatientIdAsync(PatientId),
                Times.Once());

            act.Should()
                .NotThrowAsync();

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult?.Value as Medications;

            contentResult.Should().NotBeNull();

            contentResult.Regular.Should().NotBeNullOrEmpty();
            contentResult.Regular.Should().AllBeOfType<Medication>();
            contentResult.Regular[0].Should().NotBeNull();
            contentResult.Regular[0].ScheduleType.Should().Be(MedicationScheduleType.Regular);

            contentResult.Other.Should().NotBeNullOrEmpty();
            contentResult.Other.Should().AllBeOfType<Medication>();
            contentResult.Other[0].Should().NotBeNull();
            contentResult.Other[0].ScheduleType.Should().Be(MedicationScheduleType.Other);
        }

        private IMedicationService SetupMedicationServiceForGetPatientByIdAsync()
        {
            _serviceMock = new Mock<IMedicationService>();

            _serviceMock
                .Setup(x => x.GetMedicationsByPatientIdAsync(PatientId))
                .Returns(Task.FromResult(new Medications
                {
                    Regular = new[]
                    {
                        new Medication
                        {
                            ScheduleType = MedicationScheduleType.Regular
                        }
                    },
                    Other = new[]
                    {
                        new Medication
                        {
                            ScheduleType = MedicationScheduleType.Other
                        }
                    }
                }));

            return _serviceMock.Object;
        }
    }
}
// <copyright file="AppointmentsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Controllers;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Tests.Controllers
{
    [TestFixture]
    public class AppointmentsControllerTests
    {
        private Mock<IAppointmentRepository> _appointmentRepositoryMocked;
        private Mock<ILogger<AppointmentsController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _appointmentRepositoryMocked = new Mock<IAppointmentRepository>();
            _loggerMocked = new Mock<ILogger<AppointmentsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task ScheduleAppointment_Test()
        {
            //Arrange
            var model = new ScheduleAppointmentModel
            {
                PatientId = Guid.NewGuid().ToString()
            };
            var appointment = ConfigureRepository(model);
            var appointmentsController =
                new AppointmentsController(_appointmentRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);

            appointmentsController.MockObjectValidator();

            //Act
            var actionResult = await appointmentsController.ScheduleAppointment(model);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (ScheduledAppointment)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNull();
            responseData.Patient.Identifiers.First().Should().Be(
                appointment.Patient.Identifiers.First());
        }

        private ScheduledAppointment ConfigureRepository(ScheduleAppointmentModel model)
        {
            var appointment = new ScheduledAppointment
            {
                Patient = new()
                {
                    Identifiers = new[] { model.PatientId }
                }
            };

            _appointmentRepositoryMocked
                .Setup(repository => repository.ScheduleAppointmentAsync(model))
                .Returns(Task.FromResult(appointment));

            return appointment;
        }
    }
}

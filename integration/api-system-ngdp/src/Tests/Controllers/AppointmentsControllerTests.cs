// <copyright file="AppointmentsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Testing;
using Duly.Ngdp.Api.Controllers;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Tests.Controllers
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
        public async Task GetAppointmentsForSpecificLocationTest()
        {
            //Arrange
            const string locationId = "309";
            var start = DateTimeOffset.Now;
            var end = DateTimeOffset.Now;
            var includedVisitTypes = new[] { "xxx", "xxxx", "xxxxx" };
            var appointments = SetupRepository(locationId, start, end, includedVisitTypes);

            var controller = new AppointmentsController(_loggerMocked.Object, _iWebHostEnvironment.Object, _appointmentRepositoryMocked.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.GetAppointmentsForSpecificLocation(locationId, start, end, includedVisitTypes);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<Appointment>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(appointments);
        }

        [Test]
        public async Task GetAppointmentsByCsnIdTest()
        {
            //Arrange
            const string csnId = "1";
            var appointment = SetupRepository(csnId);

            var controller = new AppointmentsController(_loggerMocked.Object, _iWebHostEnvironment.Object, _appointmentRepositoryMocked.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.GetAppointmentByCsnId(csnId);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (Appointment)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(appointment);
        }

        [Test]
        public async Task GetAppointmentsForPatientByCsnIdTest()
        {
            //Arrange
            const string csnId = "1";
            var start = DateTimeOffset.Now;
            var end = DateTimeOffset.Now;
            var statuses = new[]
            {
                AppointmentStatusParam.Arrived,
                AppointmentStatusParam.Canceled,
            };

            var appointments = SetupRepository(csnId, start, end, statuses);

            var controller = new AppointmentsController(_loggerMocked.Object, _iWebHostEnvironment.Object, _appointmentRepositoryMocked.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.GetAppointmentsForPatientByCsnId(csnId, start, end, statuses);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<Appointment>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(appointments);
        }

        [Test]
        public async Task GetReferralAppointmentsByReferralIdTest()
        {
            //Arrange
            const string referralId = "1";

            var appointments = SetupRepositoryForReferralAppointments(referralId);

            var controller = new AppointmentsController(_loggerMocked.Object, _iWebHostEnvironment.Object, _appointmentRepositoryMocked.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.GetReferralAppointmentsByReferralId(referralId);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<ReferralAppointment>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(appointments);
        }

        private IEnumerable<Appointment> SetupRepository(
            string locationId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string[] includedVisitTypes)
        {
            IEnumerable<Appointment> appointments = new Appointment[]
            {
                new()
            };

            _appointmentRepositoryMocked
                .Setup(repository =>
                    repository.GetAppointmentsForLocationByDateRangeAsync(locationId, startDate, endDate, includedVisitTypes, default))
                .ReturnsAsync(appointments);

            return appointments;
        }

        private Appointment SetupRepository(string csnId)
        {
            var appointment = new Appointment();

            _appointmentRepositoryMocked
                .Setup(repository =>
                    repository.GetAppointmentByCsnId(csnId))
                .ReturnsAsync(appointment);

            return appointment;
        }

        private IEnumerable<Appointment> SetupRepository(string csnId, DateTimeOffset startDate, DateTimeOffset endDate, AppointmentStatusParam[] statuses)
        {
            IEnumerable<Appointment> appointments = new Appointment[]
            {
                new()
            };

            _appointmentRepositoryMocked
                .Setup(repository =>
                    repository.GetAppointmentsForPatientByCsnIdAsync(csnId, startDate, endDate, statuses))
                .ReturnsAsync(appointments);

            return appointments;
        }

        private IEnumerable<ReferralAppointment> SetupRepositoryForReferralAppointments(string referralId)
        {
            IEnumerable<ReferralAppointment> appointments = new ReferralAppointment[]
            {
                new()
            };

            _appointmentRepositoryMocked
                .Setup(repository =>
                    repository.GetReferralAppointmentsByReferralIdAsync(referralId))
                .ReturnsAsync(appointments);

            return appointments;
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="AppointmentRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.Ngdp.Api.Client;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class AppointmentRepositoryTests
    {
        private Mock<IEncounterContext> _encounterContextMocked;
        private Mock<ILocationsClient> _locationsClientMocked;
        private Mock<IAppointmentsClient> _appointmentsClientMocked;
        private Mock<IClient> _commonClientMocked;
        private Mock<IMapper> _mapperMocked;

        [SetUp]
        public void Setup()
        {
            _encounterContextMocked = new Mock<IEncounterContext>();
            _locationsClientMocked = new Mock<ILocationsClient>();
            _appointmentsClientMocked = new Mock<IAppointmentsClient>();
            _commonClientMocked = new Mock<IClient>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task GetAppointmentsBySiteIdAndDateRangeAsync_Test()
        {
            //Arrange
            const string siteId = "test_id";
            var date = DateTimeOffset.UtcNow;

            var includedVisitTypes = new[] { "8001" };
            var excludedStatuses = new[] { Models.AppointmentStatusParam.Canceled };
            var ngdpExcludedStatuses = new[] { AppointmentStatusParam.Canceled };

            var ngdpAppointments = BuildNgdpAppointmentsWithLocationsClient(
                siteId,
                date,
                includedVisitTypes,
                ngdpExcludedStatuses);

            ConfigureMapper(ngdpAppointments, excludedStatuses, ngdpExcludedStatuses);

            var repositoryMocked = new AppointmentRepository(
                _encounterContextMocked.Object,
                _locationsClientMocked.Object,
                _appointmentsClientMocked.Object,
                _commonClientMocked.Object,
                _mapperMocked.Object);

            //Act
            var results = await repositoryMocked.GetAppointmentsBySiteIdAndDateRangeAsync(
                siteId,
                date.AddYears(-3),
                date.AddYears(3),
                includedVisitTypes,
                excludedStatuses);

            //Assert
            _mapperMocked
                .Verify(
                    x => x.Map<IEnumerable<AppointmentStatusParam>>(excludedStatuses),
                    Times.Once());

            _locationsClientMocked
                .Verify(
                    x => x.AppointmentsAsync(
                        siteId,
                        date.AddYears(-3),
                        date.AddYears(3),
                        includedVisitTypes,
                        It.IsAny<Guid>(),
                        ngdpExcludedStatuses,
                        default),
                    Times.Once());

            _mapperMocked
                .Verify(
                    x => x.Map<IEnumerable<Models.Appointment>>(ngdpAppointments),
                    Times.Once());

            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(ngdpAppointments.Count());
            results.First().Id.Should().Be(ngdpAppointments.First().Id);
        }

        [Test]
        public async Task GetAppointmentsForSamePatientByAppointmentIdAsync_Test()
        {
            //Arrange
            const string appointmentId = "test_id";
            var date = DateTimeOffset.UtcNow;

            var includedStatuses = new[] { Models.AppointmentStatusParam.Canceled };
            var ngdpIncludedStatuses = new[] { AppointmentStatusParam.Canceled };

            var ngdpAppointments = BuildNgdpAppointmentsWithAppointmentsClient(
                appointmentId,
                date,
                ngdpIncludedStatuses);

            ConfigureMapper(ngdpAppointments, includedStatuses, ngdpIncludedStatuses);

            var repositoryMocked = new AppointmentRepository(
                _encounterContextMocked.Object,
                _locationsClientMocked.Object,
                _appointmentsClientMocked.Object,
                _commonClientMocked.Object,
                _mapperMocked.Object);

            //Act
            var results = await repositoryMocked.GetAppointmentsForSamePatientByAppointmentIdAsync(
                appointmentId,
                date.AddYears(-3),
                date.AddYears(3),
                includedStatuses);

            //Assert
            _mapperMocked
                .Verify(
                    x => x.Map<IEnumerable<AppointmentStatusParam>>(includedStatuses),
                    Times.Once());

            _appointmentsClientMocked
                .Verify(
                    x => x.ForSamePatientAsync(
                        appointmentId,
                        date.AddYears(-3),
                        date.AddYears(3),
                        ngdpIncludedStatuses,
                        It.IsAny<Guid>(),
                        default),
                    Times.Once());

            _mapperMocked
                .Verify(
                    x => x.Map<IEnumerable<Models.Appointment>>(ngdpAppointments),
                    Times.Once());

            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(ngdpAppointments.Count());
            results.First().Id.Should().Be(ngdpAppointments.First().Id);
        }

        [Test]
        public async Task GetAppointmentByIdAsync_Test()
        {
            string appointmentId = "123";
            var ngdpAppointment = BuildAppointmentWithClient(appointmentId);

            ConfigureMapper(ngdpAppointment);

            var repositoryMocked = new AppointmentRepository(
                _encounterContextMocked.Object,
                _locationsClientMocked.Object,
                _appointmentsClientMocked.Object,
                _commonClientMocked.Object,
                _mapperMocked.Object);

            //Act
            var result = await repositoryMocked.GetAppointmentByIdAsync(appointmentId);

            //Assert

            _commonClientMocked
                .Verify(
                    x => x.AppointmentsAsync(appointmentId, It.IsAny<Guid>(), default),
                    Times.Once());

            _mapperMocked
                .Verify(
                    x => x.Map<Models.Appointment>(ngdpAppointment),
                    Times.Once());

            result.Should().NotBeNull();
            result.Id.Should().Be(ngdpAppointment.Id);
        }

        private ICollection<Appointment> BuildNgdpAppointmentsWithLocationsClient(
            string siteId,
            DateTimeOffset date,
            string[] includedVisitTypes,
            IEnumerable<AppointmentStatusParam> ngdpExcludedStatuses)
        {
            ICollection<Appointment> ngdpAppointments = new Appointment[]
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = AppointmentStatus.Completed
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = AppointmentStatus.Scheduled
                }
            };

            _locationsClientMocked
               .Setup(client => client.AppointmentsAsync(
                   siteId,
                   date.AddYears(-3),
                   date.AddYears(3),
                   includedVisitTypes,
                   It.IsAny<Guid>(),
                   ngdpExcludedStatuses,
                   default))
               .Returns(Task.FromResult(ngdpAppointments));

            return ngdpAppointments;
        }

        private ICollection<Appointment> BuildNgdpAppointmentsWithAppointmentsClient(
            string appointmentId,
            DateTimeOffset date,
            IEnumerable<AppointmentStatusParam> ngdpIncludedStatuses)
        {
            ICollection<Appointment> ngdpAppointments = new Appointment[]
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = AppointmentStatus.Completed
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = AppointmentStatus.Scheduled
                }
            };

            _appointmentsClientMocked
                .Setup(client => client.ForSamePatientAsync(
                    appointmentId,
                    date.AddYears(-3),
                    date.AddYears(3),
                    ngdpIncludedStatuses,
                    It.IsAny<Guid>(),
                    default))
                .Returns(Task.FromResult(ngdpAppointments));

            return ngdpAppointments;
        }

        private Appointment BuildAppointmentWithClient(string appointmentId)
        {
            var appointment = new Appointment()
            {
                Id = appointmentId,
                Status = AppointmentStatus.Completed
            };

            _commonClientMocked
                .Setup(client => client.AppointmentsAsync(
                    appointmentId,
                    It.IsAny<Guid>(),
                    default))
                .Returns(Task.FromResult(appointment));
            return appointment;
        }

        private void ConfigureMapper(
            IEnumerable<Appointment> ngdpAppointments,
            Models.AppointmentStatusParam[] statusParams,
            IEnumerable<AppointmentStatusParam> ngdpStatusParams)
        {
            var appointments = ngdpAppointments
                .Select(x => new Models.Appointment
                {
                    Id = x.Id
                });

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Models.Appointment>>(ngdpAppointments))
                .Returns(appointments);

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<AppointmentStatusParam>>(statusParams))
                .Returns(ngdpStatusParams);
        }

        private void ConfigureMapper(Appointment ngdpAppointment)
        {
            var appointment = new Models.Appointment()
            {
                Id = ngdpAppointment.Id,
            };
            _mapperMocked
                .Setup(mapper => mapper.Map<Models.Appointment>(ngdpAppointment))
                .Returns(appointment);
        }
    }
}

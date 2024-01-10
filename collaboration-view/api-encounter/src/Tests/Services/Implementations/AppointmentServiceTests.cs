// <copyright file="AppointmentServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Configurations;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Extensions;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class AppointmentServiceTests
    {
        private Mock<IAppointmentRepository> _repositoryMocked;
        private Mock<IMapper> _mapperMocked;
        private Mock<IPatientRepository> _patientRepositoryMocked;
        private Mock<IPractitionerRepository> _practitionerRepositoryMocked;
        private Mock<ILogger<AppointmentService>> _loggerMocked;
        private Mock<IOptionsMonitor<AppointmentOptions>> _optionsMonitorMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMocked = new Mock<IAppointmentRepository>();
            _mapperMocked = new Mock<IMapper>();
            _patientRepositoryMocked = new Mock<IPatientRepository>();
            _practitionerRepositoryMocked = new Mock<IPractitionerRepository>();
            _loggerMocked = new Mock<ILogger<AppointmentService>>();
            _optionsMonitorMock = new Mock<IOptionsMonitor<AppointmentOptions>>();
        }

        [Test]
        public async Task GetAppointmentsBySiteIdAndDateRangeAsync_EmptyResult_Test()
        {
            //Arrange
            const string siteId = "123";
            var startDate = DateTimeOffset.Now;
            var endDate = startDate.AddDays(1);

            BuildAppointmentOptions(null, null, null, null, null, null);

            var serviceMocked = new AppointmentService(
                _repositoryMocked.Object,
                _mapperMocked.Object,
                _patientRepositoryMocked.Object,
                _practitionerRepositoryMocked.Object,
                _loggerMocked.Object,
                _optionsMonitorMock.Object);

            //Act
            var results = await serviceMocked.GetAppointmentsBySiteIdAndDateRangeAsync(siteId, startDate, endDate);

            //Assert
            results.Should().BeEmpty();

            _repositoryMocked.Verify(
                r => r.GetAppointmentsBySiteIdAndDateRangeAsync(
                    siteId,
                    startDate,
                    endDate,
                    It.IsAny<string[]>(),
                    It.IsAny<AppointmentStatusParam[]>()),
                Times.Once);
        }

        [Test]
        public async Task GetAppointmentsBySiteIdAndDateRangeAsync_NonEmptyResult_SuccessPath_Test()
        {
            //Arrange
            const string siteId = "123";
            const string patientName = "Test patient";
            const string epicPatientIdentifier = "epic patient id";
            const string practitionerName = "Test practitioner";
            const string epicPractitionerIdentifier = "epic practitioner id";

            var startDate = DateTimeOffset.Now;
            var endDate = startDate.AddDays(1);

            var includedVisitTypes = new[] { "VisitType" };
            var excludedStatuses = new[] { AppointmentStatusParam.Scheduled };
            var newPatientVisitTypes = new HashSet<string>(new[] { "NewPatientVisitType" });

            BuildAppointmentOptions(includedVisitTypes, excludedStatuses,  newPatientVisitTypes, null, null, null);
            var modelsAppointments = BuildModelsAppointments(siteId, startDate, endDate, newPatientVisitTypes.First());
            BuildModelsPatients($"EXTERNAL|{modelsAppointments.First().Patient.Id}", epicPatientIdentifier);
            BuildModelsPractitioners($"EXTERNAL|{modelsAppointments.First().Practitioner.Id}", epicPractitionerIdentifier);
            ConfigureTimeSlotMapper(startDate, endDate);
            ConfigurePatientMapper(epicPatientIdentifier, patientName);
            ConfigurePractitionerMapper(epicPractitionerIdentifier, practitionerName);

            var serviceMocked = new AppointmentService(
                _repositoryMocked.Object,
                _mapperMocked.Object,
                _patientRepositoryMocked.Object,
                _practitionerRepositoryMocked.Object,
                _loggerMocked.Object,
                _optionsMonitorMock.Object);

            //Act
            var results = await serviceMocked.GetAppointmentsBySiteIdAndDateRangeAsync(siteId, startDate, endDate);

            //Assert
            results.Should().HaveCount(1);

            var firstResult = results.First();

            firstResult.Id.Should().Be(modelsAppointments.First().Id);
            firstResult.Title.Should().Be(modelsAppointments.First().Visit.TypeDisplayName);
            firstResult.Status.Should().Be(ApiContracts.AppointmentStatus.Unknown);
            firstResult.Type.Should().Be(ApiContracts.AppointmentType.OnSite);

            firstResult.TimeSlot.StartTime.Should().Be(startDate);
            firstResult.TimeSlot.EndTime.Should().Be(endDate);

            firstResult.Patient.IsNewPatient.Should().BeTrue();
            firstResult.Patient.PatientGeneralInfo.Id.Should().Be(epicPatientIdentifier);
            firstResult.Patient.PatientGeneralInfo.HumanName.FamilyName.Should().Be(patientName);

            firstResult.Practitioner.Id.Should().Be(epicPractitionerIdentifier);
            firstResult.Practitioner.HumanName.FamilyName.Should().Be(practitionerName);

            _repositoryMocked.Verify(
                r => r.GetAppointmentsBySiteIdAndDateRangeAsync(
                    siteId,
                    startDate,
                    endDate,
                    includedVisitTypes,
                    excludedStatuses),
                Times.Once);

            _patientRepositoryMocked.Verify(
                r => r.GetPatientsByIdsAsync(It.Is<string[]>(patientIds => patientIds.Length == 1)),
                Times.Once);

            _practitionerRepositoryMocked.Verify(
                r => r.GetPractitionersByIdsAsync(It.Is<string[]>(practitionerIds => practitionerIds.Length == 1)),
                Times.Once);

            _mapperMocked.Verify(
                r => r.Map<ApiContracts.TimeSlot>(It.Is<TimeSlot>(ts => ts.StartTime == startDate)),
                Times.Once);

            _mapperMocked.Verify(
                r => r.Map<ApiContracts.PatientGeneralInfo>(It.Is<PatientGeneralInfo>(p => p.Id == epicPatientIdentifier)),
                Times.Once);

            _mapperMocked.Verify(
                r => r.Map<ApiContracts.PractitionerGeneralInfo>(It.Is<PractitionerGeneralInfo>(p => p.Id == epicPractitionerIdentifier)),
                Times.Once);
        }

        [Test]
        public async Task GetAppointmentsBySiteIdAndDateRangeAsync_NoPractitioners_NoEpicCall_Test()
        {
            //Arrange
            const string siteId = "123";

            var startDate = DateTimeOffset.Now;
            var endDate = startDate.AddDays(1);

            var includedVisitTypes = new[] { "VisitType" };
            var excludedStatuses = new[] { AppointmentStatusParam.Scheduled };
            var newPatientVisitTypes = new HashSet<string>(new[] { "NewPatientVisitType" });

            BuildAppointmentOptions(includedVisitTypes, excludedStatuses, newPatientVisitTypes, null, null, null);

            var modelsAppointments = BuildModelsAppointments(siteId, startDate, endDate, newPatientVisitTypes.First());
            modelsAppointments.First().Practitioner.HumanName.GivenNames = Array.Empty<string>();

            var serviceMocked = new AppointmentService(
                _repositoryMocked.Object,
                _mapperMocked.Object,
                _patientRepositoryMocked.Object,
                _practitionerRepositoryMocked.Object,
                _loggerMocked.Object,
                _optionsMonitorMock.Object);

            //Act
            await serviceMocked.GetAppointmentsBySiteIdAndDateRangeAsync(siteId, startDate, endDate);

            //Assert
            _practitionerRepositoryMocked.Verify(
                r => r.GetPractitionersByIdsAsync(It.IsAny<string[]>()),
                Times.Never);
        }

        [Test]
        public async Task GetAppointmentsBySiteIdAndDateRangeAsync_BtgAppointment_NoPatientAndNoEpicCall_Test()
        {
            //Arrange
            const string siteId = "123";
            const string patientName = "Test patient";
            const string epicPatientIdentifier = "epic patient id";
            const string practitionerName = "Test practitioner";
            const string epicPractitionerIdentifier = "epic practitioner id";

            var startDate = DateTimeOffset.Now;
            var endDate = startDate.AddDays(1);

            var includedVisitTypes = new[] { "VisitType" };
            var excludedStatuses = new[] { AppointmentStatusParam.Scheduled };
            var newPatientVisitTypes = new HashSet<string>(new[] { "NewPatientVisitType" });

            BuildAppointmentOptions(includedVisitTypes, excludedStatuses,  newPatientVisitTypes, null, null, null);

            var modelsAppointments = BuildModelsAppointments(siteId, startDate, endDate, newPatientVisitTypes.First());
            modelsAppointments.First().IsProtectedByBtg = true;

            BuildModelsPatients($"EXTERNAL|{modelsAppointments.First().Patient.Id}", epicPatientIdentifier);
            BuildModelsPractitioners($"EXTERNAL|{modelsAppointments.First().Practitioner.Id}", epicPractitionerIdentifier);
            ConfigureTimeSlotMapper(startDate, endDate);
            ConfigurePatientMapper(epicPatientIdentifier, patientName);
            ConfigurePractitionerMapper(epicPractitionerIdentifier, practitionerName);

            var serviceMocked = new AppointmentService(
                _repositoryMocked.Object,
                _mapperMocked.Object,
                _patientRepositoryMocked.Object,
                _practitionerRepositoryMocked.Object,
                _loggerMocked.Object,
                _optionsMonitorMock.Object);

            //Act
            var results = await serviceMocked.GetAppointmentsBySiteIdAndDateRangeAsync(siteId, startDate, endDate);

            //Assert
            results.Should().HaveCount(1);

            var firstResult = results.First();

            firstResult.Patient.PatientGeneralInfo.Id.Should().Be(modelsAppointments.First().Patient.Id);
            firstResult.Patient.PatientGeneralInfo.HumanName.FamilyName.Should().Be("Restricted");

            _patientRepositoryMocked.Verify(
                r => r.GetPatientsByIdsAsync(It.IsAny<string[]>()),
                Times.Never);
        }

        [TestCase(false, ApiContracts.AppointmentType.OnSite)]
        [TestCase(true, ApiContracts.AppointmentType.Telehealth)]
        public async Task GetAppointmentsBySiteIdAndDateRangeAsync_NonEmptyResult_CheckAppointmentType_Test(
            bool source,
            ApiContracts.AppointmentType expected)
        {
            //Arrange
            const string siteId = "123";

            var startDate = DateTimeOffset.Now;
            var endDate = startDate.AddDays(1);

            var includedVisitTypes = new[] { "VisitType" };
            var excludedStatuses = new[] { AppointmentStatusParam.Scheduled };
            var newPatientVisitTypes = new HashSet<string>(new[] { "NewPatientVisitType" });

            BuildAppointmentOptions(includedVisitTypes, excludedStatuses,  newPatientVisitTypes, null, null, null);
            BuildModelsAppointments(siteId, startDate, endDate, newPatientVisitTypes.First(), isTelehealthVisit: source);

            var serviceMocked = new AppointmentService(
                _repositoryMocked.Object,
                _mapperMocked.Object,
                _patientRepositoryMocked.Object,
                _practitionerRepositoryMocked.Object,
                _loggerMocked.Object,
                _optionsMonitorMock.Object);

            //Act
            var results = await serviceMocked.GetAppointmentsBySiteIdAndDateRangeAsync(siteId, startDate, endDate);

            //Assert
            results.Should().HaveCount(1);
            results.First().Type.Should().Be(expected);
        }

        [Test]
        public async Task GetAppointmentsBySiteIdAndDateRangeAsync_NoEpicPatient_LogRecordAndDefaultPatient_Test()
        {
            //Arrange
            const string siteId = "123";
            const string patientName = "Test patient";
            const string epicPatientIdentifier = "epic patient id";
            const string practitionerName = "Test practitioner";
            const string epicPractitionerIdentifier = "epic practitioner id";
            const string ngdpPatientId = "ngdpPatient";
            const string unknownPatientName = "Unknown";

            var startDate = DateTimeOffset.Now;
            var endDate = startDate.AddDays(1);

            var includedVisitTypes = new[] { "VisitType" };
            var excludedStatuses = new[] { AppointmentStatusParam.Scheduled };
            var newPatientVisitTypes = new HashSet<string>(new[] { "NewPatientVisitType" });

            BuildAppointmentOptions(includedVisitTypes, excludedStatuses,  newPatientVisitTypes, null, null, null);
            var modelsAppointments = BuildModelsAppointments(siteId, startDate, endDate, newPatientVisitTypes.First(), ngdpPatientId);
            BuildModelsPatients("non-existent", epicPatientIdentifier);
            BuildModelsPractitioners($"EXTERNAL|{modelsAppointments.First().Practitioner.Id}", epicPractitionerIdentifier);
            ConfigureTimeSlotMapper(startDate, endDate);
            ConfigurePatientMapper(epicPatientIdentifier, patientName);
            ConfigurePractitionerMapper(epicPractitionerIdentifier, practitionerName);

            var serviceMocked = new AppointmentService(
                _repositoryMocked.Object,
                _mapperMocked.Object,
                _patientRepositoryMocked.Object,
                _practitionerRepositoryMocked.Object,
                _loggerMocked.Object,
                _optionsMonitorMock.Object);

            //Act
            var results = await serviceMocked.GetAppointmentsBySiteIdAndDateRangeAsync(siteId, startDate, endDate);

            //Assert
            results.Should().HaveCount(1);

            var firstResult = results.First();

            firstResult.Patient.PatientGeneralInfo.Id.Should().Be(ngdpPatientId);
            firstResult.Patient.PatientGeneralInfo.HumanName.FamilyName.Should().Be(unknownPatientName);

            _loggerMocked.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Test]
        public async Task GetAppointmentsBySiteIdAndDateRangeAsync_NoEpicPractitioner_LogRecord_Test()
        {
            //Arrange
            const string siteId = "123";
            const string patientName = "Test patient";
            const string epicPatientIdentifier = "epic patient id";
            const string practitionerName = "Test practitioner";
            const string epicPractitionerIdentifier = "epic practitioner id";
            const string ngdpPractitionerId = "ngdpPractitioner";

            var startDate = DateTimeOffset.Now;
            var endDate = startDate.AddDays(1);

            var includedVisitTypes = new[] { "VisitType" };
            var excludedStatuses = new[] { AppointmentStatusParam.Scheduled };
            var newPatientVisitTypes = new HashSet<string>(new[] { "NewPatientVisitType" });

            BuildAppointmentOptions(includedVisitTypes, excludedStatuses,  newPatientVisitTypes, null, null, null);
            var modelsAppointments = BuildModelsAppointments(siteId, startDate, endDate, newPatientVisitTypes.First(), practitionerId: ngdpPractitionerId);
            BuildModelsPatients($"EXTERNAL|{modelsAppointments.First().Patient.Id}", epicPatientIdentifier);
            BuildModelsPractitioners($"non-existent", epicPractitionerIdentifier);
            ConfigureTimeSlotMapper(startDate, endDate);
            ConfigurePatientMapper(epicPatientIdentifier, patientName);
            ConfigurePractitionerMapper(epicPractitionerIdentifier, practitionerName);

            var serviceMocked = new AppointmentService(
                _repositoryMocked.Object,
                _mapperMocked.Object,
                _patientRepositoryMocked.Object,
                _practitionerRepositoryMocked.Object,
                _loggerMocked.Object,
                _optionsMonitorMock.Object);

            //Act
            var results = await serviceMocked.GetAppointmentsBySiteIdAndDateRangeAsync(siteId, startDate, endDate);

            //Assert
            results.Should().HaveCount(1);

            var firstResult = results.First();

            firstResult.Practitioner.Id.Should().Be(ngdpPractitionerId);

            _loggerMocked.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Test]
        public async Task GetAppointmentsForSamePatientByAppointmentIdAsync_Test_NoOtherNgdpAppointments()
        {
            //Arrange
            const string practitionerName = "Test practitioner";
            const string epicPractitionerIdentifier = "epic_practitioner_id";
            const string appointmentId = "123";
            var startDate = DateTimeOffset.UtcNow.AddYears(-3);
            var endDate = DateTimeOffset.UtcNow.AddYears(3);

            var includedPatientAppointmentStatuses = new[]
            {
                AppointmentStatusParam.Completed,
                AppointmentStatusParam.Canceled,
                AppointmentStatusParam.NoShow,
                AppointmentStatusParam.Scheduled
            };
            var recentPatientAppointmentStatuses = new HashSet<AppointmentStatus>(new[]
            {
                AppointmentStatus.Completed,
                AppointmentStatus.Canceled,
                AppointmentStatus.NoShow
            });
            var upcomingPatientAppointmentStatuses = new HashSet<AppointmentStatus>(new[]
            {
                AppointmentStatus.Scheduled
            });

            ConfigureTimeSlotMapper(startDate, endDate);
            ConfigurePractitionerMapper(epicPractitionerIdentifier, practitionerName);
            BuildModelsPractitioners($"EXTERNAL|{epicPractitionerIdentifier}", epicPractitionerIdentifier);
            BuildAppointmentOptions(
                null,
                null,
                null,
                includedPatientAppointmentStatuses,
                recentPatientAppointmentStatuses,
                upcomingPatientAppointmentStatuses);
            var modelsAppointments = BuildModelsAppointments(
                appointmentId,
                startDate,
                endDate,
                epicPractitionerIdentifier,
                includedPatientAppointmentStatuses,
                recentPatientAppointmentStatuses,
                upcomingPatientAppointmentStatuses,
                true);

            var serviceMocked = new AppointmentService(
                _repositoryMocked.Object,
                _mapperMocked.Object,
                _patientRepositoryMocked.Object,
                _practitionerRepositoryMocked.Object,
                _loggerMocked.Object,
                _optionsMonitorMock.Object);

            //Act
            var results = await serviceMocked.GetAppointmentsForSamePatientByAppointmentIdAsync(appointmentId);

            //Assert
            _repositoryMocked.Verify(
                x => x.GetAppointmentsForSamePatientByAppointmentIdAsync(
                    appointmentId,
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<DateTimeOffset>(),
                    includedPatientAppointmentStatuses),
                Times.Once);

            results.Should().NotBeNull();
            results.RecentAppointments.Should().NotBeNull();
            results.RecentAppointments.Should().HaveCount(0);
            results.UpcomingAppointments.Should().NotBeNull();
            results.UpcomingAppointments.Should().HaveCount(0);
        }

        [Test]
        public async Task GetAppointmentsForSamePatientByAppointmentIdAsync_Test()
        {
            //Arrange
            const string practitionerName = "Test practitioner";
            const string epicPractitionerIdentifier = "epic_practitioner_id";
            const string appointmentId = "123";
            var startDate = DateTimeOffset.UtcNow.AddYears(-3);
            var endDate = DateTimeOffset.UtcNow.AddYears(3);

            var includedPatientAppointmentStatuses = new[]
            {
                AppointmentStatusParam.Completed,
                AppointmentStatusParam.Canceled,
                AppointmentStatusParam.NoShow,
                AppointmentStatusParam.Scheduled
            };
            var recentPatientAppointmentStatuses = new HashSet<AppointmentStatus>(new[]
            {
                AppointmentStatus.Completed,
                AppointmentStatus.Canceled,
                AppointmentStatus.NoShow
            });
            var upcomingPatientAppointmentStatuses = new HashSet<AppointmentStatus>(new[]
            {
                AppointmentStatus.Scheduled
            });

            ConfigureTimeSlotMapper(startDate, endDate);
            ConfigurePractitionerMapper(epicPractitionerIdentifier, practitionerName);
            BuildModelsPractitioners($"EXTERNAL|{epicPractitionerIdentifier}", epicPractitionerIdentifier);
            BuildAppointmentOptions(
                null,
                null,
                null,
                includedPatientAppointmentStatuses,
                recentPatientAppointmentStatuses,
                upcomingPatientAppointmentStatuses);
            var modelsAppointments = BuildModelsAppointments(
                appointmentId,
                startDate,
                endDate,
                epicPractitionerIdentifier,
                includedPatientAppointmentStatuses,
                recentPatientAppointmentStatuses,
                upcomingPatientAppointmentStatuses);

            var serviceMocked = new AppointmentService(
                _repositoryMocked.Object,
                _mapperMocked.Object,
                _patientRepositoryMocked.Object,
                _practitionerRepositoryMocked.Object,
                _loggerMocked.Object,
                _optionsMonitorMock.Object);

            var expectedPractitionerGeneralInfoCount = modelsAppointments.Count(x => x.Id != appointmentId)
                                                       + recentPatientAppointmentStatuses.Count
                                                       + (2 * upcomingPatientAppointmentStatuses.Select(s => s).Count(s => s == AppointmentStatus.Scheduled));

            //Act
            var results = await serviceMocked.GetAppointmentsForSamePatientByAppointmentIdAsync(appointmentId);

            //Assert
            _repositoryMocked.Verify(
                x => x.GetAppointmentsForSamePatientByAppointmentIdAsync(
                    appointmentId,
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<DateTimeOffset>(),
                    includedPatientAppointmentStatuses),
                Times.Once);

            _practitionerRepositoryMocked.Verify(
                r => r.GetPractitionersByIdsAsync(
                    It.Is<string[]>(practitionerIds => practitionerIds.Length == 1)),
                Times.Once);

            _mapperMocked.Verify(
                r => r.Map<ApiContracts.PractitionerGeneralInfo>(
                    It.Is<PractitionerGeneralInfo>(p => p.Id == epicPractitionerIdentifier)),
                Times.Exactly(expectedPractitionerGeneralInfoCount));

            results.Should().NotBeNull();
            results.RecentAppointments.Should().NotBeNull();
            results.RecentAppointments.Should().HaveCount(3);
            var newestAppointmentsGroup = results.RecentAppointments.First();
            var oldestAppointmentsGroup = results.RecentAppointments.Last();
            newestAppointmentsGroup.NearestAppointmentDate.Should()
                .BeAfter(oldestAppointmentsGroup.NearestAppointmentDate);

            results.UpcomingAppointments.Should().NotBeNull();
            results.UpcomingAppointments.Should().HaveCount(2);
            var nearestAppointmentsGroup = results.UpcomingAppointments.First();
            var farthestAppointmentsGroup = results.UpcomingAppointments.Last();
            farthestAppointmentsGroup.NearestAppointmentDate.Should()
                .BeAfter(nearestAppointmentsGroup.NearestAppointmentDate);
        }

        [Test]
        public async Task GetPractitionerByAppointmentIdAsync_Test()
        {
            //Arrange
            const string appointmentId = "test-appointment-id";
            const string practitionerName = "Test practitioner";
            const string ngdpPractitionerIdentifier = "ngdp-practitioner-id";
            const string epicPractitionerIdentifier = "epic-practitioner-id";
            BuildAppointmentOptions(null, null, null, null, null, null);
            var modelAppointment = BuildModelsAppointment(appointmentId, ngdpPractitionerIdentifier);
            BuildModelsPractitioners(modelAppointment.Practitioner.Id.ToIdWithExternalPrefix(), epicPractitionerIdentifier);
            ConfigurePractitionerMapper(epicPractitionerIdentifier, practitionerName);

            var serviceMocked = new AppointmentService(
                _repositoryMocked.Object,
                _mapperMocked.Object,
                _patientRepositoryMocked.Object,
                _practitionerRepositoryMocked.Object,
                _loggerMocked.Object,
                _optionsMonitorMock.Object);

            //Act
            var result = await serviceMocked.GetPractitionerByAppointmentIdAsync(appointmentId);

            //Assert
            _repositoryMocked.Verify(
                    repository => repository.GetAppointmentByIdAsync(appointmentId),
                    Times.Once);

            _practitionerRepositoryMocked.Verify(
                repository => repository.GetPractitionersByIdsAsync(new[] { modelAppointment.Practitioner.Id.ToIdWithExternalPrefix() }),
                Times.Once);

            _mapperMocked.Verify(
                mapper => mapper.Map<Contracts.PractitionerGeneralInfo>(It.IsAny<PractitionerGeneralInfo>()),
                Times.Once);

            result.Should().NotBeNull();
            result.Id.Should().Be(epicPractitionerIdentifier);
            result.HumanName.FamilyName.Should().Be(practitionerName);
        }

        [Test]
        public async Task GetPractitionerByAppointmentIdAsync_NoEpicPractitioner_Test()
        {
            //Arrange
            const string appointmentId = "test-appointment-id";
            const string practitionerName = "Test practitioner";
            const string ngdpPractitionerIdentifier = "ngdp-practitioner-id";
            const string epicPractitionerIdentifier = "epic-practitioner-id";
            BuildAppointmentOptions(null, null, null, null, null, null);
            var modelAppointment = BuildModelsAppointment(appointmentId, ngdpPractitionerIdentifier);
            BuildModelsPractitioners("non-existent", epicPractitionerIdentifier);
            ConfigurePractitionerMapper(epicPractitionerIdentifier, practitionerName);

            var serviceMocked = new AppointmentService(
                _repositoryMocked.Object,
                _mapperMocked.Object,
                _patientRepositoryMocked.Object,
                _practitionerRepositoryMocked.Object,
                _loggerMocked.Object,
                _optionsMonitorMock.Object);

            //Act
            var result = await serviceMocked.GetPractitionerByAppointmentIdAsync(appointmentId);

            //Assert
            _repositoryMocked.Verify(
                repository => repository.GetAppointmentByIdAsync(appointmentId),
                Times.Once);

            _practitionerRepositoryMocked.Verify(
                repository => repository.GetPractitionersByIdsAsync(new[] { modelAppointment.Practitioner.Id.ToIdWithExternalPrefix() }),
                Times.Once);

            _loggerMocked.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);

            result.Should().NotBeNull();
            result.Id.Should().Be(ngdpPractitionerIdentifier);
        }

        private IEnumerable<Appointment> BuildModelsAppointments(
            string appointmentId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string practitionerId,
            AppointmentStatusParam[] includedAppointmentStatuses,
            HashSet<AppointmentStatus> recentAppointmentStatuses,
            HashSet<AppointmentStatus> upcomingAppointmentStatuses,
            bool noOtherAppointments = false)
        {
            var appointmentStatusItems = new Dictionary<int, AppointmentStatus>
            {
                { 0, AppointmentStatus.Completed }
            };

            if (!noOtherAppointments)
            {
                for (var i = 0; i < 2; i++)
                {
                    foreach (var status in recentAppointmentStatuses)
                    {
                        appointmentStatusItems.Add(appointmentStatusItems.Count, status);
                    }

                    foreach (var status in upcomingAppointmentStatuses)
                    {
                        appointmentStatusItems.Add(appointmentStatusItems.Count, status);
                    }
                }
            }

            var appointmentStartTimeBase = startDate
                .AddTicks((endDate - startDate).Ticks / 2);
            var appointments = appointmentStatusItems
                .Select(item =>
                {
                    var (index, status) = item;
                    var appointmentStartTime = appointmentStartTimeBase
                        .AddDays(!upcomingAppointmentStatuses.Contains(status) ? -(2 * index) : 2 * index);
                    return new Appointment()
                    {
                        Id = index == 0 ? appointmentId : Guid.NewGuid().ToString(),
                        Status = status,
                        TimeSlot = new TimeSlot
                        {
                            StartTime = appointmentStartTime,
                            EndTime = appointmentStartTime.AddHours(1)
                        },
                        Visit = new AppointmentVisit
                        {
                            TypeDisplayName = status != AppointmentStatus.Scheduled ? $"Appointment type {status}" : $"Appointment type {status}_{index}"
                        },
                        Practitioner = new AppointmentPractitioner
                        {
                            Id = practitionerId ?? "256",
                            HumanName = new HumanName
                            {
                                GivenNames = new[] { "GivenName" }
                            }
                        }
                    };
                })
                .ToArray();

            _repositoryMocked
                .Setup(repo => repo.GetAppointmentsForSamePatientByAppointmentIdAsync(
                    appointmentId,
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<DateTimeOffset>(),
                    includedAppointmentStatuses))
                .Returns(Task.FromResult(appointments.AsEnumerable()));

            return appointments;
        }

        private IEnumerable<Appointment> BuildModelsAppointments(
            string siteId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string visitType,
            string patientId = null,
            string practitionerId = null,
            bool isTelehealthVisit = false)
        {
            IEnumerable<Appointment> appointments = new Appointment[]
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    TimeSlot = new()
                    {
                        StartTime = startDate,
                        EndTime = endDate
                    },
                    Visit = new()
                    {
                        TypeDisplayName = "Appointment title",
                        TypeId = visitType
                    },
                    Practitioner = new()
                    {
                        Id = practitionerId ?? "256",
                        HumanName = new()
                        {
                            GivenNames = new[] { "GivenName" }
                        }
                    },
                    Patient = new()
                    {
                        Id = patientId ?? "156"
                    },
                    IsTelehealthVisit = isTelehealthVisit
                }
            };

            _repositoryMocked
                .Setup(repo => repo.GetAppointmentsBySiteIdAndDateRangeAsync(
                    siteId,
                    startDate,
                    endDate,
                    It.IsAny<string[]>(),
                    It.IsAny<AppointmentStatusParam[]>()))
                .Returns(Task.FromResult(appointments));

            return appointments;
        }

        private Appointment BuildModelsAppointment(string appointmentId, string practitionerId)
        {
            var appointment = new Appointment()
            {
                Practitioner = new()
                {
                    Id = practitionerId,
                    HumanName = new()
                    {
                        GivenNames = new[] { "GivenName" }
                    }
                },
            };

            _repositoryMocked
                .Setup(repository => repository.GetAppointmentByIdAsync(appointmentId))
                .Returns(Task.FromResult(appointment));
            return appointment;
        }

        private void BuildAppointmentOptions(
            string[] includedVisitTypes,
            AppointmentStatusParam[] excludedAppointmentStatuses,
            HashSet<string> patientVisitTypes,
            AppointmentStatusParam[] includedPatientAppointmentStatuses,
            HashSet<AppointmentStatus> recentPatientAppointmentStatuses,
            HashSet<AppointmentStatus> upcomingPatientAppointmentStatuses)
        {
            var appointmentOptions = new AppointmentOptionsMock(
                includedVisitTypes,
                excludedAppointmentStatuses,
                patientVisitTypes,
                includedPatientAppointmentStatuses,
                recentPatientAppointmentStatuses,
                upcomingPatientAppointmentStatuses);

            _optionsMonitorMock.Setup(monitor => monitor.CurrentValue).Returns(appointmentOptions);
        }

        private void BuildModelsPatients(string ngdpPatientIdentifier, string epicPatientIdentifier)
        {
            IEnumerable<Patient> patients = new Patient[]
            {
                new()
                {
                    PatientGeneralInfo = new()
                    {
                        Id = epicPatientIdentifier
                    },
                    Identifiers = new[] { ngdpPatientIdentifier, "randomString" }
                }
            };

            _patientRepositoryMocked
                .Setup(repo => repo.GetPatientsByIdsAsync(It.IsAny<string[]>()))
                .Returns(Task.FromResult(patients));
        }

        private void BuildModelsPractitioners(string practitionerIdentifier, string epicPatientIdentifier)
        {
            IEnumerable<PractitionerGeneralInfo> practitioners = new PractitionerGeneralInfo[]
            {
                new()
                {
                    Id = epicPatientIdentifier,
                    Identifiers = new[] { practitionerIdentifier, "randomString" }
                }
            };

            _practitionerRepositoryMocked
                .Setup(repo => repo.GetPractitionersByIdsAsync(It.IsAny<string[]>()))
                .Returns(Task.FromResult(practitioners));
        }

        private void ConfigureTimeSlotMapper(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var timeSlot = new ApiContracts.TimeSlot
            {
                StartTime = startDate,
                EndTime = endDate
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<ApiContracts.TimeSlot>(It.IsAny<TimeSlot>()))
                .Returns(timeSlot);
        }

        private void ConfigurePatientMapper(string patientId, string patientName)
        {
            var patient = new ApiContracts.PatientGeneralInfo
            {
                Id = patientId,
                HumanName = new()
                {
                    FamilyName = patientName
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<ApiContracts.PatientGeneralInfo>(It.IsAny<PatientGeneralInfo>()))
                .Returns(patient);
        }

        private void ConfigurePractitionerMapper(string practitionerId, string practitionerName)
        {
            var practitioner = new ApiContracts.PractitionerGeneralInfo
            {
                Id = practitionerId,
                HumanName = new()
                {
                    FamilyName = practitionerName
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<ApiContracts.PractitionerGeneralInfo>(It.IsAny<PractitionerGeneralInfo>()))
                .Returns(practitioner);
        }
    }
}

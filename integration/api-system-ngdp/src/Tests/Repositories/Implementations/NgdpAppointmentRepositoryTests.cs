// <copyright file="NgdpAppointmentRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Common.Infrastructure.Exceptions;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Models;
using Duly.Ngdp.Api.Repositories.Implementations;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using Duly.Ngdp.Contracts;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class NgdpAppointmentRepositoryTests
    {
        private Mock<IMapper> _mapperMocked;
        private Mock<IAppointmentAdapter> _adapterMocked;
        private Mock<ITimeZoneConverter> _timeZoneConverter;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IAppointmentAdapter>();
            _mapperMocked = new Mock<IMapper>();

            _timeZoneConverter = new Mock<ITimeZoneConverter>();

            _timeZoneConverter
                .Setup(tz => tz.ToCstDateTime(It.IsAny<DateTimeOffset>()))
                .Returns<DateTimeOffset>(dt => dt.DateTime);

            _timeZoneConverter
                .Setup(tz => tz.ToCstDateTimeOffset(It.IsAny<DateTime>()))
                .Returns<DateTime>(dt => new DateTimeOffset(dt, TimeSpan.Zero));
        }

        [Test]
        public async Task GetAppointmentsForLocationByDateRangeAsync_Test()
        {
            //Arrange
            const string departmentId = "309";
            var start = DateTimeOffset.Now;
            var end = DateTimeOffset.Now;
            var includedVisitTypes = new[] { "xxx", "xxxx", "xxxxx" };

            var appointments = SetupAdapter(departmentId, start, end, includedVisitTypes);
            var systemAppointments = SetupMapper(appointments);

            var repository = new NgdpAppointmentRepository(_adapterMocked.Object, _mapperMocked.Object, _timeZoneConverter.Object);

            //Act
            var result = await repository
                .GetAppointmentsForLocationByDateRangeAsync(departmentId, start, end, includedVisitTypes);

            //Assert
            result.Should().BeEquivalentTo(systemAppointments);
        }

        [Test]
        public async Task GetAppointmentsForLocationByDateRangeAsync_ConvertAppointmentStatusParam_Test()
        {
            //Arrange
            const string departmentId = "309";
            var start = DateTimeOffset.Now;
            var end = DateTimeOffset.Now;
            var includedVisitTypes = new[] { "xxx", "xxxx", "xxxxx" };
            var excludedAppointmentStatuses = new[]
            {
                AppointmentStatusParam.Arrived
            };

            var appointments = SetupAdapter(departmentId, start, end, includedVisitTypes);
            var systemAppointments = SetupMapper(appointments);

            var repository = new NgdpAppointmentRepository(_adapterMocked.Object, _mapperMocked.Object, _timeZoneConverter.Object);

            //Act
            var result = await repository
                .GetAppointmentsForLocationByDateRangeAsync(departmentId, start, end, includedVisitTypes, excludedAppointmentStatuses);

            //Assert
            result.Should().BeEquivalentTo(systemAppointments);
        }

        [Test]
        public async Task GetAppointmentByCsnIdAsync_Test()
        {
            //Arrange
            const string csnId = "1";

            var appointment = SetupAdapter(csnId);
            var systemAppointment = SetupMapper(appointment);

            var repository = new NgdpAppointmentRepository(_adapterMocked.Object, _mapperMocked.Object, _timeZoneConverter.Object);

            //Act
            var result = await repository.GetAppointmentByCsnId(csnId);

            //Assert
            result.Should().BeEquivalentTo(systemAppointment);
        }

        [Test]
        public async Task GetAppointmentByCsnIdAsync_ShouldThrowEntityNotFound_Test()
        {
            //Arrange
            const string csnId = "1";

            _adapterMocked
                .Setup(adapter => adapter.FindAppointmentByCsnIdAsync(csnId))
                .ReturnsAsync((AdapterModels.Appointment)null);

            var repository = new NgdpAppointmentRepository(_adapterMocked.Object, _mapperMocked.Object, _timeZoneConverter.Object);

            //Assert
            await repository.Invoking(x => x.GetAppointmentByCsnId(csnId)).Should().ThrowAsync<EntityNotFoundException>();
        }

        [Test]
        public async Task GetAppointmentsForPatientByCsnIdAsync_Test()
        {
            //Arrange
            const string csnId = "1";
            var start = DateTimeOffset.Now;
            var end = DateTimeOffset.Now;
            var includedAppointmentStatuses = new[]
            {
                AppointmentStatusParam.Arrived
            };

            var appointments = SetupAdapterForPatient(csnId, start, end);
            var systemAppointments = SetupMapper(appointments);

            var repository = new NgdpAppointmentRepository(_adapterMocked.Object, _mapperMocked.Object, _timeZoneConverter.Object);

            //Act
            var result = await repository.GetAppointmentsForPatientByCsnIdAsync(csnId, start, end, includedAppointmentStatuses);

            //Assert
            result.Should().BeEquivalentTo(systemAppointments);
        }

        [Test]
        public async Task GetReferralAppointmentsByReferralIdAsync_Test()
        {
            //Arrange
            const string referralId = "1";

            var appointments = SetupAdapterWithReferralAppointments(referralId);
            var systemAppointments = SetupMapper(appointments);

            var repository = new NgdpAppointmentRepository(_adapterMocked.Object, _mapperMocked.Object, _timeZoneConverter.Object);

            //Act
            var result = await repository.GetReferralAppointmentsByReferralIdAsync(referralId);

            //Assert
            result.Should().BeEquivalentTo(systemAppointments);
        }

        private IEnumerable<AdapterModels.Appointment> SetupAdapter(
            string departmentId,
            DateTimeOffset start,
            DateTimeOffset end,
            string[] includedVisitTypes)
        {
            IEnumerable<AdapterModels.Appointment> appointments = new AdapterModels.Appointment[]
            {
                new()
            };

            _adapterMocked
                .Setup(adapter => adapter.FindAppointmentsAsync(
                    It.Is<AppointmentSearchCriteria>(criteria =>
                        criteria.DepartmentId == departmentId
                        && criteria.AppointmentTimeLowerBound == start.DateTime
                        && criteria.AppointmentTimeUpperBound == end.DateTime
                        && criteria.IncludedVisitTypeIds.Length == includedVisitTypes.Length
                        && criteria.ExcludedStatuses.Length == 0)))
                .ReturnsAsync(appointments);

            return appointments;
        }

        private AdapterModels.Appointment SetupAdapter(string csnId)
        {
            var appointment = new AdapterModels.Appointment();

            _adapterMocked
                .Setup(adapter => adapter.FindAppointmentByCsnIdAsync(csnId))
                .ReturnsAsync(appointment);

            return appointment;
        }

        private IEnumerable<AdapterModels.Appointment> SetupAdapterForPatient(string csnId, DateTimeOffset start, DateTimeOffset end)
        {
            IEnumerable<AdapterModels.Appointment> appointments = new AdapterModels.Appointment[]
            {
                new()
            };

            _adapterMocked
                .Setup(adapter => adapter.FindAppointmentsForPatientByCsnIdAsync(
                    It.Is<AppointmentSearchCriteria>(criteria =>
                        criteria.CsnId == csnId
                        && criteria.AppointmentTimeLowerBound == start.DateTime
                        && criteria.AppointmentTimeUpperBound == end.DateTime
                        && criteria.IncludedStatuses.Length == 0)))
                .ReturnsAsync(appointments);

            return appointments;
        }

        private IEnumerable<AdapterModels.ReferralAppointment> SetupAdapterWithReferralAppointments(string referralId)
        {
            IEnumerable<AdapterModels.ReferralAppointment> appointments = new AdapterModels.ReferralAppointment[]
            {
                new()
            };

            _adapterMocked
                .Setup(adapter => adapter.FindReferralAppointmentsByReferralIdAsync(referralId))
                .ReturnsAsync(appointments);

            return appointments;
        }

        private IEnumerable<Contracts.Appointment> SetupMapper(IEnumerable<AdapterModels.Appointment> appointments)
        {
            IEnumerable<Contracts.Appointment> systemAppointments = new Contracts.Appointment[]
            {
                new()
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Contracts.Appointment>>(appointments))
                .Returns(systemAppointments);

            return systemAppointments;
        }

        private Contracts.Appointment SetupMapper(AdapterModels.Appointment appointments)
        {
            Contracts.Appointment systemAppointment = new();

            _mapperMocked
                .Setup(mapper => mapper.Map<Contracts.Appointment>(appointments))
                .Returns(systemAppointment);

            return systemAppointment;
        }

        private IEnumerable<Contracts.ReferralAppointment> SetupMapper(IEnumerable<AdapterModels.ReferralAppointment> appointments)
        {
            IEnumerable<Contracts.ReferralAppointment> systemAppointments = new Contracts.ReferralAppointment[]
            {
                new()
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Contracts.ReferralAppointment>>(appointments))
                .Returns(systemAppointments);

            return systemAppointments;
        }
    }
}

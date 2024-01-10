// <copyright file="AppointmentAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.DataAccess.Contexts.Interfaces;
using Duly.Ngdp.Adapter.Adapters.Implementations;
using Duly.Ngdp.Adapter.Adapters.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ngdp.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class AppointmentAdapterTests
    {
        private const string FindAppointmentsStoredProcedureName = "[dulycv].[uspAppointmentsSelect]";
        private const string FindAppointmentByCsnIdStoredProcedureName = "[dulycv].[uspAppointmentSelectSingleByCsnId]";
        private const string FindAppointmentsForPatientByCsnIdStoredProcedureName = "[dulycv].[uspAppointmentsSelectForPatientByCsnId]";

        private Mock<IDapperContext> _mockedDapperContext;

        [SetUp]
        public void Setup()
        {
            _mockedDapperContext = new Mock<IDapperContext>();
        }

        [Test]
        public async Task FindAppointmentsAsync_ArgumentNullException_Test()
        {
            //Arrange
            var adapter = new AppointmentAdapter(_mockedDapperContext.Object);

            //Act
            Func<Task> action = async () => await adapter.FindAppointmentsAsync(null);

            //Assert
            var result = await action.Should().ThrowAsync<ArgumentNullException>();
            result.Which.Message.Should().Be("Value cannot be null. (Parameter 'searchCriteria')");
        }

        [Test]
        public async Task FindAppointmentsAsync_Success_Test()
        {
            //Arrange
            var adapter = new AppointmentAdapter(_mockedDapperContext.Object);
            var searchCriteria = new AppointmentSearchCriteria
            {
                IncludedVisitTypeIds = Array.Empty<string>(),
                ExcludedStatuses = Array.Empty<AppointmentStatus>()
            };
            var appointments = SetupDapperContext();

            //Act
            var result = await adapter.FindAppointmentsAsync(searchCriteria);

            //Assert
            result.Should().BeEquivalentTo(appointments);
        }

        [Test]
        public async Task FindAppointmentByCsnIdAsync_Success_Test()
        {
            //Arrange
            var adapter = new AppointmentAdapter(_mockedDapperContext.Object);
            var csnId = "1";
            var appointments = SetupDapperContext();

            //Act
            var result = await adapter.FindAppointmentByCsnIdAsync(csnId);

            //Assert
            result.Should().BeEquivalentTo(appointments.SingleOrDefault());
        }

        [Test]
        public async Task FindAppointmentsForPatientByCsnIdAsync_ArgumentNullException_Test()
        {
            //Arrange
            var adapter = new AppointmentAdapter(_mockedDapperContext.Object);

            //Act
            Func<Task> action = async () => await adapter.FindAppointmentsForPatientByCsnIdAsync(null);

            //Assert
            var result = await action.Should().ThrowAsync<ArgumentNullException>();
            result.Which.Message.Should().Be("Value cannot be null. (Parameter 'searchCriteria')");
        }

        [Test]
        public async Task FindAppointmentsForPatientByCsnIdAsync_Success_Test()
        {
            //Arrange
            var adapter = new AppointmentAdapter(_mockedDapperContext.Object);
            var searchCriteria = new AppointmentSearchCriteria
            {
                IncludedStatuses = new[] { AppointmentStatus.Arrived }
            };
            var appointments = SetupDapperContext();

            //Act
            var result = await adapter.FindAppointmentsForPatientByCsnIdAsync(searchCriteria);

            //Assert
            result.Should().BeEquivalentTo(appointments);
        }

        private IEnumerable<Appointment> SetupDapperContext()
        {
            IEnumerable<Appointment> appointments = new Appointment[]
            {
                new()
            };

            _mockedDapperContext
                .Setup(context =>
                    context.QueryAsync<Appointment>(FindAppointmentsStoredProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(appointments);

            _mockedDapperContext
                .Setup(context =>
                    context.QueryAsync<Appointment>(FindAppointmentByCsnIdStoredProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(appointments);

            _mockedDapperContext
                .Setup(context =>
                    context.QueryAsync<Appointment>(FindAppointmentsForPatientByCsnIdStoredProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(appointments);

            return appointments;
        }
    }
}

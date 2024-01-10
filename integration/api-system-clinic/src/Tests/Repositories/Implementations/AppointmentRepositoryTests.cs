// <copyright file="AppointmentRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Contracts;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wipfli.Adapter.Adapters.Interfaces;
using Wipfli.Adapter.Client;
using Wipfli.Adapter.Configuration;

namespace Duly.Clinic.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class AppointmentRepositoryTests
    {
        private Mock<IOptionsMonitor<PrivateApiOptions>> _privateApiOptionsMonitorMocked;
        private Mock<IScheduleAdapter> _adapterMocked;
        private Mock<IMapper> _mapperMocked;

        [SetUp]
        public void SetUp()
        {
            _privateApiOptionsMonitorMocked = new Mock<IOptionsMonitor<PrivateApiOptions>>();
            _adapterMocked = new Mock<IScheduleAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task ScheduleAppointmentAsync_Test()
        {
            //Arrange
            var model = new ScheduleAppointmentModel
            {
                Date = DateTime.Now.AddDays(5).Date,
                Time = TimeSpan.Parse("14:15:00"),
                PatientId = "EXTERNAL|7650074",
                ProviderId = "External|1405",
                DepartmentId = "External|25069",
                VisitTypeId = "External|2147"
            };
            var appointment = ConfigureProvider(model);
            ConfigureMapper(appointment);

            _privateApiOptionsMonitorMocked
                .Setup(x => x.CurrentValue)
                .Returns(new PrivateApiOptions
                {
                    BypassAppointmentCreation = true
                });

            IAppointmentRepository repositoryMocked = new AppointmentRepository(
                    _privateApiOptionsMonitorMocked.Object,
                    _adapterMocked.Object,
                    _mapperMocked.Object);

            //Act
            var results = await repositoryMocked.ScheduleAppointmentAsync(model);

            //Assert
            results.Should().NotBeNull();
            results.Patient.Identifiers.First().Should().Be(appointment.Appointment.Patient.IDs.First().ID);
            results.Provider.Identifiers.First().Should().Be(appointment.Appointment.Provider.IDs.First().ID);
        }

        private ScheduledAppointmentWithInsurance ConfigureProvider(ScheduleAppointmentModel model)
        {
            ScheduledAppointmentWithInsurance appointment = new()
            {
                Appointment = new()
                {
                    Patient = new()
                    {
                        IDs = new List<Identity>
                        {
                            new()
                            {
                                ID = model.PatientId.SplitIdentifier().ID
                            }
                        }
                    },
                    Provider = new()
                    {
                        IDs = new List<Identity>
                        {
                            new()
                            {
                                ID = model.ProviderId.SplitIdentifier().ID
                            }
                        }
                    }
                }
            };

            _adapterMocked
                .Setup(provider => provider
                    .ScheduleAppointment(It.Is<ScheduleAppointmentWithInsuranceRequest>(x =>
                        x.PatientID == model.PatientId.SplitIdentifier().ID &&
                        x.ProviderID == model.ProviderId.SplitIdentifier().ID)))
                .Returns(Task.FromResult(appointment));

            return appointment;
        }

        private void ConfigureMapper(ScheduledAppointmentWithInsurance appointment)
        {
            ScheduledAppointment appointmentSystem = new()
            {
                Patient = new()
                {
                    Identifiers = new[]
                    {
                        appointment.Appointment.Patient.IDs.First().ID
                    }
                },
                Provider = new()
                {
                    Identifiers = new[]
                    {
                        appointment.Appointment.Provider.IDs.First().ID
                    }
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<ScheduledAppointment>(appointment.Appointment))
                .Returns(appointmentSystem);
        }
    }
}

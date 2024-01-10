// <copyright file="ScheduleAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wipfli.Adapter.Adapters.Implementations;
using Wipfli.Adapter.Adapters.Interfaces;
using Wipfli.Adapter.Client;

namespace Wipfli.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class ScheduleAdapterTests
    {
        private readonly Mock<IWipfliClient> _mockedClient = new();

        [Test]
        public async Task GetScheduleForProvider_Test()
        {
            //Arrange
            var startDate = DateTime.Now.AddDays(3);
            var providerId = "1405";

            var searchCriteria = new ScheduleDaysForProviderSearchCriteria
            {
                ProviderID = providerId,
                StartDate = startDate,
            };

            var schedule = new Schedule
            {
                ScheduleDays = new List<ScheduleDay>
                {
                    new()
                    {
                        Date = startDate,
                        Provider = new Provider
                        {
                            IDs = new List<Identity>
                            {
                                new() { ID = providerId }
                            }
                        }
                    }
                }
            };

            _mockedClient
                .Setup(x =>
                    x.GetScheduleDaysForProvider(
                        It.Is<ScheduleDaysForProviderSearchCriteria>(y =>
                            y.StartDate == searchCriteria.StartDate &&
                            y.ProviderID == searchCriteria.ProviderID),
                        default))
                .Returns(Task.FromResult(schedule));

            IScheduleAdapter adapter = new ScheduleAdapter(_mockedClient.Object);

            //Act
            var result = await adapter.GetScheduleForProvider(searchCriteria);

            //Assert
            result.Should().NotBeNull();
            result.ScheduleDays.First().Date.Should().Be(startDate);
            result.ScheduleDays.First().Provider.IDs.First().ID.Should().Be(providerId);
        }

        [Test]
        public async Task ScheduleAppointment_Test()
        {
            //Arrange
            var date = DateTime.Now.AddDays(3);
            var providerId = "1405";

            var request = new ScheduleAppointmentWithInsuranceRequest
            {
                Date = date,
                ProviderID = providerId,
            };

            var appointment = new ScheduledAppointmentWithInsurance
            {
                Appointment = new()
                {
                    Date = date,
                    Provider = new()
                    {
                        IDs = new List<Identity>
                        {
                            new() { ID = providerId }
                        }
                    }
                }
            };

            _mockedClient
                .Setup(x =>
                    x.ScheduleAppointmentWithInsurance(
                        It.Is<ScheduleAppointmentWithInsuranceRequest>(y =>
                            y.Date == request.Date &&
                            y.ProviderID == request.ProviderID),
                        default))
                .Returns(Task.FromResult(appointment));

            IScheduleAdapter adapter = new ScheduleAdapter(_mockedClient.Object);

            //Act
            var result = await adapter.ScheduleAppointment(request);

            //Assert
            result.Should().NotBeNull();
            result.Appointment.Date.Should().Be(date);
            result.Appointment.Provider.IDs.First().ID.Should().Be(providerId);
        }
    }
}

// <copyright file="ScheduleRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Api.Repositories.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wipfli.Adapter.Adapters.Interfaces;
using Wipfli.Adapter.Client;
using ScheduleDay = Wipfli.Adapter.Client.ScheduleDay;
using Slot = Wipfli.Adapter.Client.Slot;

namespace Duly.Clinic.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class ScheduleRepositoryTests
    {
        private Mock<IScheduleAdapter> _adapterMocked;
        private Mock<IMapper> _mapperMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IScheduleAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task GetPatientGeneralInfoByIdAsyncTest()
        {
            //Arrange
            const string providerId = "external|101";
            const string departmentId = "unknown|202";
            const string visitTypeId = "external|303";
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(5);

            var schedule = ConfigureProvider();
            ConfigureMapper(schedule);
            IScheduleRepository repositoryMocked = new ScheduleRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var results = await repositoryMocked.GetScheduleAsync(startDate, endDate, providerId, departmentId, visitTypeId);

            //Assert
            results.Should().NotBeNull();
            results.First().Slots.First().ArrivalTime.Should()
                .Be(schedule.ScheduleDays.First().Slots.First().ArrivalTime);
            results.First().Slots.First().DisplayTime.Should()
                .Be(schedule.ScheduleDays.First().Slots.First().DisplayTime);
            results.First().Slots.First().Time.Should()
                .Be(schedule.ScheduleDays.First().Slots.First().Time);
        }

        private Schedule ConfigureProvider()
        {
            Schedule schedule = new()
            {
                ScheduleDays = new List<ScheduleDay>
                {
                    new ()
                    {
                        Slots = new List<Slot>
                        {
                            new ()
                            {
                                ArrivalTime = TimeSpan.FromHours(5),
                                DisplayTime = TimeSpan.FromHours(6),
                                Time = TimeSpan.FromHours(7),
                            }
                        }
                    }
                }
            };

            _adapterMocked
                .Setup(provider =>
                    provider.GetScheduleForProvider(It.IsAny<ScheduleDaysForProviderSearchCriteria>()))
                .Returns(Task.FromResult(schedule));

            return schedule;
        }

        private void ConfigureMapper(Schedule schedule)
        {
            IEnumerable<Contracts.ScheduleDay> systemSchedule = new List<Contracts.ScheduleDay>
            {
                new()
                {
                    Slots = new Contracts.Slot[]
                    {
                        new ()
                        {
                            ArrivalTime = TimeSpan.FromHours(5),
                            DisplayTime = TimeSpan.FromHours(6),
                            Time = TimeSpan.FromHours(7),
                        }
                    }
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Contracts.ScheduleDay>>(schedule.ScheduleDays))
                .Returns(systemSchedule);
        }
    }
}

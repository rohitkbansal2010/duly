// <copyright file="ScheduleDayConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Slot = Wipfli.Adapter.Client.Slot;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class ScheduleDayConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertTest()
        {
            //Arrange
            Wipfli.Adapter.Client.ScheduleDay source = new()
            {
                Date = new DateTime(2020, 1, 11),
                Slots = new List<Slot>
                {
                    new()
                    {
                        ArrivalTime = TimeSpan.FromHours(5),
                        DisplayTime = TimeSpan.FromHours(6),
                        Time = TimeSpan.FromHours(7),
                    }
                }
            };

            //Act
            var result = Mapper.Map<ScheduleDay>(source);

            //Assert
            result.Should().NotBeNull();
            result.Date.Should().Be(source.Date);
            result.Slots.First().ArrivalTime.Should().Be(source.Slots.First().ArrivalTime);
            result.Slots.First().DisplayTime.Should().Be(source.Slots.First().DisplayTime);
            result.Slots.First().Time.Should().Be(source.Slots.First().Time);
        }
    }
}

// <copyright file="TimeZoneConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Ngdp.Api.Configurations;
using Duly.Ngdp.Api.Repositories.Mappings.Converters;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;

namespace Duly.Ngdp.Api.Tests.Repositories.Mappings.Converters
{
    [TestFixture]
    public class TimeZoneConverterTests
    {
        private static readonly NgdpTimeZone NgdpTimeZone = new()
        {
            DefaultName =
#if DEBUG
                "Central Standard Time"
#else
                "America/Chicago"
#endif
        };

        private Mock<IOptionsMonitor<NgdpTimeZone>> _ngdpTimeZoneOptionsMonitorMocked;

        [SetUp]
        public void SetUp()
        {
            _ngdpTimeZoneOptionsMonitorMocked = new Mock<IOptionsMonitor<NgdpTimeZone>>();

            _ngdpTimeZoneOptionsMonitorMocked
                .Setup(a => a.CurrentValue)
                .Returns(NgdpTimeZone);
        }

        [TestCase("2022-03-14T14:00+3", "2022-03-14T06:00")]
        [TestCase("2022-02-14T14:00+3", "2022-02-14T05:00")]
        public void ToCstDateTimeTest(string inputDateTime, string expectedDateTime)
        {
            //Arrange
            var dateTimeOffset = DateTimeOffset.Parse(inputDateTime);
            ITimeZoneConverter converter = new TimeZoneConverter(_ngdpTimeZoneOptionsMonitorMocked.Object);

            //Act
            var result = converter.ToCstDateTime(dateTimeOffset);

            //Assert
            result.Should().Be(DateTime.Parse(expectedDateTime));
        }

        [TestCase("2022-03-14T06:00", "2022-03-14T06:00-5")]
        [TestCase("2022-02-14T06:00", "2022-02-14T06:00-6")]
        public void ToCstDateTimeOffsetTest(string inputDateTime, string expectedDateTime)
        {
            //Arrange
            var dateTime = DateTime.Parse(inputDateTime);
            ITimeZoneConverter converter = new TimeZoneConverter(_ngdpTimeZoneOptionsMonitorMocked.Object);

            //Act
            var result = converter.ToCstDateTimeOffset(dateTime);

            //Assert
            result.Should().Be(DateTimeOffset.Parse(expectedDateTime));
        }
    }
}

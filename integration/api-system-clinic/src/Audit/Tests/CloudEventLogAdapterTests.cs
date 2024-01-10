// <copyright file="CloudEventLogAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Messaging;
using Duly.Clinic.Audit.Ingestion.Configuration;
using Duly.Clinic.Audit.Ingestion.Interfaces;
using Duly.Clinic.Audit.Ingestion.Models;
using Duly.Clinic.Audit.Ingestion.Services;
using FluentAssertions;
using LogAnalytics.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Duly.Clinic.Audit.Ingestion.Tests
{
    [TestFixture]
    public class CloudEventLogAdapterTests
    {
        private Mock<ILogger> _loggerMocked;
        private Mock<ILogAnalyticsEntryBuilder> _logAnalyticsEntryBuilderMocked;
        private Mock<ILogAnalyticsClient> _logAnalyticsClientMocked;
        private Mock<IOptionsMonitor<LogAnalyticsOptions>> _logAnalyticsOptionsMonitorMocked;

        [SetUp]
        public void SetUp()
        {
            _loggerMocked = new Mock<ILogger>();
            _logAnalyticsClientMocked = new Mock<ILogAnalyticsClient>();
            _logAnalyticsEntryBuilderMocked = new Mock<ILogAnalyticsEntryBuilder>();
            _logAnalyticsOptionsMonitorMocked = new Mock<IOptionsMonitor<LogAnalyticsOptions>>();

            _logAnalyticsEntryBuilderMocked
                .Setup(x => x.BuildLogAnalyticsEntry(
                    It.Is<CloudEvent>(x => x != null)))
                .Returns(new LogAnalyticsEntry
                {
                    Action = "Action"
                });

            _logAnalyticsEntryBuilderMocked
                .Setup(x => x.BuildLogAnalyticsEntry(
                    It.Is<CloudEvent>(x => x.Subject == null)))
                .Returns(default(LogAnalyticsEntry));

            _logAnalyticsClientMocked
                .Setup(x => x.SendLogEntry(
                    It.Is<LogAnalyticsEntry>(x => x != null),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _logAnalyticsClientMocked
                .Setup(x => x.SendLogEntry(
                    It.Is<LogAnalyticsEntry>(x => x == null),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromException(new ApplicationException("Testing")));

            _logAnalyticsOptionsMonitorMocked
                .Setup(x => x.CurrentValue)
                .Returns(new LogAnalyticsOptions { LogAnalyticsEntryType = "testLog" });
        }

        [Test]
        public async Task SendCloudEventAsyncTest()
        {
            //Arrange
            var json = "{\"specversion\":\"1.0\",\"id\":\"<guid-generated-by-azure-event-grid-sdk>\",\"type\":\"<value-matching-to-type-element>\",\"subject\":\"<value-matching-to-action-element>\",\"source\":\"<value-of-appid-claim-from-jwt>\",\"time\":\"2022-01-10T15:14:39.4589254Z\",\"datacontenttype\":\"application/json\",\"data\":{\"eventTime\":\"2022-01-10T15:13:39.4589254Z\",\"correlationToken\":\"correlation-id\",\"user\":{\"firstName\":\"value-of-given_name-claim-from-jwt\",\"lastName\":\"value-of-family_name-claim-from-jwt\",\"upn\":\"value-of-upn-claim-from-jwt\",\"roles\":[\"tbd\"]},\"clientId\":\"value-of-appid-claim-from-jwt\",\"action\":\"read/update/execute/insert/...\",\"type\":\"request/response/exception/...\",\"serviceName\":\"epic-service-name\",\"serviceVersion\":\"optional-version-of-epic-service\",\"serviceHost\":\"epic-base-url-host-name\",\"operation\":\"epic-operation-name\",\"meta\":{\"httpResponseCode\":200}}}";
            var cloudEvent = CloudEvent.Parse(BinaryData.FromString(json));

            var adapter = new CloudEventLogAdapter(
                _logAnalyticsEntryBuilderMocked.Object,
                _logAnalyticsClientMocked.Object,
                _logAnalyticsOptionsMonitorMocked.Object);

            //Act
            var result = await adapter.SendCloudEventAsync(cloudEvent, _loggerMocked.Object);

            //Assert
            _logAnalyticsEntryBuilderMocked.Verify(
                x => x.BuildLogAnalyticsEntry(
                    It.Is<CloudEvent>(x => x.Id == "<guid-generated-by-azure-event-grid-sdk>")),
                Times.Once());

            _logAnalyticsClientMocked.Verify(
                x => x.SendLogEntry(
                    It.Is<LogAnalyticsEntry>(x => x != null),
                    "testLog",
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once());

            result.Should().BeTrue();
        }

        [Test]
        public async Task SendCloudEventAsyncTest_NotSent()
        {
            //Arrange

            var adapter = new CloudEventLogAdapter(
                _logAnalyticsEntryBuilderMocked.Object,
                _logAnalyticsClientMocked.Object,
                _logAnalyticsOptionsMonitorMocked.Object);

            //Act
            var result = await adapter.SendCloudEventAsync(
                new CloudEvent(string.Empty, string.Empty, null),
                _loggerMocked.Object);

            //Assert
            _logAnalyticsEntryBuilderMocked.Verify(
                x => x.BuildLogAnalyticsEntry(
                    It.Is<CloudEvent>(x => x != null)),
                Times.Once());

            _logAnalyticsClientMocked.Verify(
                x => x.SendLogEntry(
                    It.Is<LogAnalyticsEntry>(x => x == null),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once());

            result.Should().BeFalse();
        }
    }
}

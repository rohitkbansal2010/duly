// <copyright file="LogAnalyticsEntryBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Azure.Messaging;
using Duly.Clinic.Audit.Ingestion.Exceptions;
using Duly.Clinic.Audit.Ingestion.Services;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Duly.Clinic.Audit.Ingestion.Tests
{
    [TestFixture]
    public class LogAnalyticsEntryBuilderTests
    {
        [Test]
        public void BuildLogAnalyticsEntryTest()
        {
            //Arrange
            var json = "{\"specversion\":\"1.0\",\"id\":\"<guid-generated-by-azure-event-grid-sdk>\",\"type\":\"<value-matching-to-type-element>\",\"subject\":\"<value-matching-to-action-element>\",\"source\":\"<value-of-appid-claim-from-jwt>\",\"time\":\"2022-01-10T15:14:39.4589254Z\",\"datacontenttype\":\"application/json\",\"data\":{\"eventTime\":\"2022-01-10T15:13:39.4589254Z\",\"correlationToken\":\"correlation-id\",\"user\":{\"firstName\":\"value-of-given_name-claim-from-jwt\",\"lastName\":\"value-of-family_name-claim-from-jwt\",\"upn\":\"value-of-upn-claim-from-jwt\",\"roles\":[\"tbd\"]},\"clientId\":\"value-of-appid-claim-from-jwt\",\"action\":\"read/update/execute/insert/...\",\"type\":\"request/response/exception/...\",\"serviceName\":\"epic-service-name\",\"serviceVersion\":\"optional-version-of-epic-service\",\"serviceHost\":\"epic-base-url-host-name\",\"operation\":\"epic-operation-name\",\"meta\":{\"httpResponseCode\":200}}}";
            var cloudEvent = CloudEvent.Parse(BinaryData.FromString(json));

            LogAnalyticsEntryBuilder builder = new LogAnalyticsEntryBuilder();

            //Act
            var result = builder.BuildLogAnalyticsEntry(cloudEvent);

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void BuildLogAnalyticsEntryTest_Throw_ArgumentNullException()
        {
            //Arrange

            LogAnalyticsEntryBuilder builder = new LogAnalyticsEntryBuilder();

            //Act
            Action action = () => builder.BuildLogAnalyticsEntry(null);

            //Assert
            var result = action.Should().Throw<ArgumentNullException>();
            result.Which.Message.Should().StartWith("Value cannot be null.");
        }

        [Test]
        public void BuildLogAnalyticsEntryTest_Throw_CloudEventWithoutData()
        {
            //Arrange
            var json = "{\"specversion\":\"1.0\",\"id\":\"<guid-generated-by-azure-event-grid-sdk>\",\"type\":\"<value-matching-to-type-element>\",\"subject\":\"<value-matching-to-action-element>\",\"source\":\"<value-of-appid-claim-from-jwt>\",\"time\":\"2022-01-10T15:14:39.4589254Z\",\"datacontenttype\":\"application/json\",\"data\":null}";
            var cloudEvent = CloudEvent.Parse(BinaryData.FromString(json));

            LogAnalyticsEntryBuilder builder = new LogAnalyticsEntryBuilder();

            //Act
            Action action = () => builder.BuildLogAnalyticsEntry(cloudEvent);

            //Assert
            var result = action.Should().Throw<LogAnalyticsEntryBuilderException>();
            result.Which.Message.Should().StartWith("The cloud event data field is null.");
        }

        [Test]
        public void BuildLogAnalyticsEntryTest_WithoutAdditionalData()
        {
            //Arrange
            var json = "{\"specversion\":\"1.0\",\"id\":\"<guid-generated-by-azure-event-grid-sdk>\",\"type\":\"<value-matching-to-type-element>\",\"subject\":\"<value-matching-to-action-element>\",\"source\":\"<value-of-appid-claim-from-jwt>\",\"time\":\"2022-01-10T15:14:39.4589254Z\",\"datacontenttype\":\"application/json\",\"data\":{\"eventTime\":\"2022-01-10T15:13:39.4589254Z\",\"correlationToken\":\"correlation-id\",\"clientId\":\"value-of-appid-claim-from-jwt\",\"action\":\"read/update/execute/insert/...\",\"type\":\"request/response/exception/...\",\"serviceName\":\"epic-service-name\",\"serviceVersion\":\"optional-version-of-epic-service\",\"serviceHost\":\"epic-base-url-host-name\",\"operation\":\"epic-operation-name\"}}";
            var cloudEvent = CloudEvent.Parse(BinaryData.FromString(json));

            LogAnalyticsEntryBuilder builder = new LogAnalyticsEntryBuilder();

            //Act
            var result = builder.BuildLogAnalyticsEntry(cloudEvent);

            //Assert
            result.Should().NotBeNull();
        }
    }
}

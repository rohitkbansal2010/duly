// -----------------------------------------------------------------------
// <copyright file="IntakeTest.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Azure.Core.Serialization;
using Azure.Messaging;
using Duly.Clinic.Audit.Ingestion.Exceptions;
using Duly.Clinic.Audit.Ingestion.Interfaces;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Duly.Clinic.Audit.Ingestion.Tests
{
    [TestFixture]
    public class IntakeTest
    {
        private Mock<HttpRequestData> _requestMocked;
        private Mock<ILogger> _loggerMocked;
        private Mock<ICloudEventLogAdapter> _cloudEventLogAdapterMocked;

        [SetUp]
        public void SetUp()
        {
            _requestMocked = new Mock<HttpRequestData>(MockBehavior.Strict, Mock.Of<FunctionContext>());
            _loggerMocked = new Mock<ILogger>(MockBehavior.Default);

            _cloudEventLogAdapterMocked = new Mock<ICloudEventLogAdapter>();
            _cloudEventLogAdapterMocked
                .Setup(x => x.SendCloudEventAsync(It.IsAny<CloudEvent>(), _loggerMocked.Object))
                .Returns(Task.FromResult(true));

            _requestMocked
                .Setup(r => r.CreateResponse())
                .Returns(
                    () =>
                    {
                        var services = new ServiceCollection();
                        services.Configure<WorkerOptions>(
                            c =>
                            {
                                c.Serializer = new JsonObjectSerializer();
                            });

                        var context = new Mock<FunctionContext>();
                        context.SetupProperty(c => c.InstanceServices, services.BuildServiceProvider());

                        var response = new Mock<HttpResponseData>(context.Object);
                        response.SetupProperty(r => r.Headers, new HttpHeadersCollection());
                        response.SetupProperty(r => r.StatusCode);
                        response.SetupProperty(r => r.Body, new MemoryStream());

                        return response.Object;
                    });

            _requestMocked
                .SetupGet(x => x.Url)
                .Returns(new Uri("http://duly-np.digital"));
        }

        [Test]
        public async Task ValidationRequestTest()
        {
            //Arrange
            _requestMocked
                .SetupGet(x => x.Method)
                .Returns(HttpMethod.Options.Method);

            _requestMocked
                .SetupGet(x => x.Headers)
                .Returns(
                    new HttpHeadersCollection()
                    {
                        { "WebHook-Request-Origin", "eventgrid.azure.net" }
                    });

            var invoker = new Intake(_cloudEventLogAdapterMocked.Object);

            //Act
            var result = await invoker.GetCloudEventFromEventGridTopicAsync(
                _requestMocked.Object,
                CreateFunctionContext(_loggerMocked.Object, _cloudEventLogAdapterMocked.Object));

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task ValidationRequestTest_BadRequest()
        {
            //Arrange
            _requestMocked
                .SetupGet(x => x.Method)
                .Returns(HttpMethod.Options.Method);

            _requestMocked
                .SetupGet(x => x.Headers)
                .Returns(
                    new HttpHeadersCollection());

            var invoker = new Intake(_cloudEventLogAdapterMocked.Object);

            //Act
            var result = await invoker.GetCloudEventFromEventGridTopicAsync(
                _requestMocked.Object,
                CreateFunctionContext(_loggerMocked.Object, _cloudEventLogAdapterMocked.Object));

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CloudEventRequestTest()
        {
            //Arrange
            _requestMocked
                .SetupGet(x => x.Method)
                .Returns(HttpMethod.Post.Method);

            var body = @"{
                ""specversion"": ""1.0"",
                ""type"": ""Microsoft.Storage.BlobCreated"",
                ""source"": ""/Microsoft.Storage/storageAccounts/{storage-account}"",
                ""id"": ""9aeb0fdf-c01e-0131-0922-9eb54906e209"",
                ""time"": ""2019-11-18T15:13:39.4589254Z"",
                ""subject"": ""blobServices/default/containers/{storage-container}/blobs/{new-file}"",
                ""datacontenttype"": ""application/json"",
                ""data"": {
                   ""api"": ""PutBlockList""
                   }
                 }";

            _requestMocked
                .SetupGet(x => x.Body)
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes(body)));

            var invoker = new Intake(_cloudEventLogAdapterMocked.Object);

            //Act
            var result = await invoker.GetCloudEventFromEventGridTopicAsync(
                _requestMocked.Object,
                CreateFunctionContext(_loggerMocked.Object, _cloudEventLogAdapterMocked.Object));

            //Assert
            _cloudEventLogAdapterMocked.Verify(
                x => x.SendCloudEventAsync(
                    It.Is<CloudEvent>(e => e.Id == "9aeb0fdf-c01e-0131-0922-9eb54906e209"),
                    _loggerMocked.Object),
                Times.Once());

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            _cloudEventLogAdapterMocked.VerifyAll();
        }

        [Test]
        public async Task CloudEventRequestTest_WithException()
        {
            //Arrange
            _requestMocked
                .SetupGet(x => x.Method)
                .Returns(HttpMethod.Post.Method);

            var body = string.Empty;

            _requestMocked
                .SetupGet(x => x.Body)
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes(body)));

            var invoker = new Intake(_cloudEventLogAdapterMocked.Object);

            //Act
            var result = await invoker.GetCloudEventFromEventGridTopicAsync(
                _requestMocked.Object,
                CreateFunctionContext(_loggerMocked.Object, _cloudEventLogAdapterMocked.Object));

            //Assert
            _cloudEventLogAdapterMocked.Verify(
                x => x.SendCloudEventAsync(
                    It.IsAny<CloudEvent>(),
                    _loggerMocked.Object),
                Times.Never);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CloudEventServiceBusTriggerTest()
        {
            //Arrange
            var jsonCloudEvent = @"{
                ""specversion"": ""1.0"",
                ""type"": ""Microsoft.Storage.BlobCreated"",
                ""source"": ""/Microsoft.Storage/storageAccounts/{storage-account}"",
                ""id"": ""9aeb0fdf-c01e-0131-0922-9eb54906e209"",
                ""time"": ""2019-11-18T15:13:39.4589254Z"",
                ""subject"": ""blobServices/default/containers/{storage-container}/blobs/{new-file}"",
                ""datacontenttype"": ""application/json"",
                ""data"": {
                   ""api"": ""PutBlockList""
                   }
                 }";

            var invoker = new Intake(_cloudEventLogAdapterMocked.Object);

            //Act
            await invoker.GetCloudEventFromServiceBusAsync(
                jsonCloudEvent,
                CreateFunctionContext(_loggerMocked.Object, _cloudEventLogAdapterMocked.Object));

            //Assert
            _cloudEventLogAdapterMocked.Verify(
                x => x.SendCloudEventAsync(
                    It.Is<CloudEvent>(e => e.Id == "9aeb0fdf-c01e-0131-0922-9eb54906e209"),
                    _loggerMocked.Object),
                Times.Once());

            _cloudEventLogAdapterMocked.VerifyAll();
        }

        [Test]
        public async Task CloudEventServiceBusTriggerTest_WithException()
        {
            //Arrange
            var jsonCloudEvent = @"{
                ""specversion"": ""1.0"",
                ""type"": ""Microsoft.Storage.BlobCreated"",
                ""source"": ""/Microsoft.Storage/storageAccounts/{storage-account}"",
                ""id"": ""9aeb0fdf-c01e-0131-0922-9eb54906e209"",
                ""time"": ""2019-11-18T15:13:39.4589254Z"",
                ""subject"": ""blobServices/default/containers/{storage-container}/blobs/{new-file}"",
                ""datacontenttype"": ""application/json""
                 }";

            _cloudEventLogAdapterMocked
                .Setup(x => x.SendCloudEventAsync(
                    It.Is<CloudEvent>(c => c.Data == null),
                    _loggerMocked.Object))
                .Returns(Task.FromResult(false));

            var invoker = new Intake(_cloudEventLogAdapterMocked.Object);

            //Act
            Func<Task> action = async () => await invoker.GetCloudEventFromServiceBusAsync(
                jsonCloudEvent,
                CreateFunctionContext(_loggerMocked.Object, _cloudEventLogAdapterMocked.Object));

            //Assert
            var result = await action.Should().ThrowExactlyAsync<LogAnalyticsEntryBuilderException>();
            result.Which.Message.Should().Be("Failure during attempt to send an event into log analytics workspace.");
        }

        private static FunctionContext CreateFunctionContext(ILogger logger, ICloudEventLogAdapter cloudEventLogAdapter)
        {
            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(p => p.CreateLogger(It.IsAny<string>())).Returns(logger);

            var instanceServices = new Mock<IServiceProvider>();
            instanceServices.Setup(p => p.GetService(
                It.Is<Type>(x => x == typeof(ICloudEventLogAdapter)))).Returns(cloudEventLogAdapter);
            instanceServices.Setup(p => p.GetService(
                It.Is<Type>(x => x == typeof(ILoggerFactory)))).Returns(loggerFactory.Object);

            var context = new Mock<FunctionContext>();
            context.Setup(p => p.InstanceServices).Returns(instanceServices.Object);

            return context.Object;
        }
    }
}
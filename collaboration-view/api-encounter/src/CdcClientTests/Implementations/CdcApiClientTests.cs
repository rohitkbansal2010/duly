// <copyright file="CdcApiClientTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.CdcClient.Implementations;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Duly.CollaborationView.CdcClient.Tests.Implementations
{
    [TestFixture]
    public class CdcApiClientTests
    {
        private const string ContentModel = "CDC_IMMUN_VAC_MODEL";
        private const string RouteSuffixGetContentModelCatalogs = "contentmodel/catalogs?";
        private const string RouteSuffixFindContentModelCatalogRelationsByCodes = "relation?";

        [Test]
        public async Task GetContentModelCatalogsAsync_Test_NullContentModel()
        {
            //Arrange
            var json = "[]";

            var httpMessageHandlerMock = SetupMockedHttpMessageHandler(
                json,
                RouteSuffixGetContentModelCatalogs);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost:1234");
            var client = new CdcApiClient(httpClient);

            //Act
            Func<Task> action = async () => await client.GetContentModelCatalogsAsync(null);

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task GetContentModelCatalogsAsync_Test_ReadObjectResponseError()
        {
            //Arrange
            var json = "[{\"CatalogUID\":}]";

            var httpMessageHandlerMock = SetupMockedHttpMessageHandler(
                json,
                RouteSuffixGetContentModelCatalogs);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost:1234");
            var client = new CdcApiClient(httpClient);

            //Act
            Func<Task> action = async () => await client.GetContentModelCatalogsAsync(ContentModel);

            //Assert
            await action.Should().ThrowAsync<ApiException>();
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public async Task GetContentModelCatalogsAsync_Test_WithErrorStatusCode(HttpStatusCode statusCode)
        {
            //Arrange
            var json = "[]";

            var httpMessageHandlerMock = SetupMockedHttpMessageHandler(
                json,
                RouteSuffixGetContentModelCatalogs,
                statusCode);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost:1234");
            var client = new CdcApiClient(httpClient);

            //Act
            Func<Task> action = async () => await client.GetContentModelCatalogsAsync(ContentModel);

            //Assert
            await action.Should().ThrowAsync<ApiException>();
        }

        [Test]
        public async Task GetContentModelCatalogsAsync_Test()
        {
            //Arrange
            var json = "[{\"CatalogUID\":\"53993d3c-e5fd-e611-a44f-b917d3d5053d\",\"CatalogName\":\"AMA CPT Immunization Subset\",\"CatalogVersion\":\"1.0\",\"Mnemonic\":\"CDC_IMMUN_AMA_CPT\",\"LastPublishedDate\":\"2021-12-17T18:37:35\",\"LastUpdatedDate\":\"2021-12-17T18:37:51.88\",\"TermCount\":684},{\"CatalogUID\":\"1e077ba6-f8d5-e711-8172-0e8e465e7726\",\"CatalogName\":\"CDC Vaccine Administered (CVX)\",\"CatalogVersion\":\"1.0\",\"Mnemonic\":\"CDC_IMMUN_CVX\",\"LastPublishedDate\":\"2021-12-17T18:37:35\",\"LastUpdatedDate\":\"2021-12-17T18:37:51.88\",\"TermCount\":689},{\"CatalogUID\":\"19eef0b1-fcd5-e711-9af6-001c422a1053\",\"CatalogName\":\"CDC Vaccine Group\",\"CatalogVersion\":\"1.0\",\"Mnemonic\":\"CDC_IMMUN_VAC_GRP\",\"LastPublishedDate\":\"2021-12-17T18:37:35\",\"LastUpdatedDate\":\"2021-12-17T18:37:51.88\",\"TermCount\":43},{\"CatalogUID\":\"d3c2f9f4-fbd5-e711-8172-0e8e465e7726\",\"CatalogName\":\"CDC Vaccine Information Statements (VIS)\",\"CatalogVersion\":\"1.0\",\"Mnemonic\":\"CDC_IMMUN_VIS\",\"LastPublishedDate\":\"2021-12-17T18:37:35\",\"LastUpdatedDate\":\"2021-12-17T18:37:51.88\",\"TermCount\":123}]";

            var httpMessageHandlerMock = SetupMockedHttpMessageHandler(
                json,
                RouteSuffixGetContentModelCatalogs);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost:1234");
            var client = new CdcApiClient(httpClient);

            //Act
            var result = await client.GetContentModelCatalogsAsync(ContentModel);
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(4);
        }

        [Test]
        public async Task FindContentModelCatalogRelationsByCodesAsync_Test_NullContentModel()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "withoutGroupName"
            };
            var catalogUID = "1e077ba6-f8d5-e711-8172-0e8e465e7726";
            var json = "[]";

            var httpMessageHandlerMock = SetupMockedHttpMessageHandler(
                json,
                RouteSuffixGetContentModelCatalogs);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost:1234");
            var client = new CdcApiClient(httpClient);

            //Act
            Func<Task> action = async () => await client.FindContentModelCatalogRelationsByCodesAsync(
                null,
                catalogUID,
                cvxCodes);

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task FindContentModelCatalogRelationsByCodesAsync_Test_NullCatalogUID()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "withoutGroupName"
            };
            var json = "[]";

            var httpMessageHandlerMock = SetupMockedHttpMessageHandler(
                json,
                RouteSuffixGetContentModelCatalogs);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost:1234");
            var client = new CdcApiClient(httpClient);

            //Act
            Func<Task> action = async () => await client.FindContentModelCatalogRelationsByCodesAsync(
                ContentModel,
                null,
                cvxCodes);

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task FindContentModelCatalogRelationsByCodesAsync_Test_NullCodes()
        {
            //Arrange
            var catalogUID = "1e077ba6-f8d5-e711-8172-0e8e465e7726";
            var json = "[]";

            var httpMessageHandlerMock = SetupMockedHttpMessageHandler(
                json,
                RouteSuffixGetContentModelCatalogs);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost:1234");
            var client = new CdcApiClient(httpClient);

            //Act
            Func<Task> action = async () => await client.FindContentModelCatalogRelationsByCodesAsync(
                ContentModel,
                catalogUID,
                null);

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.InternalServerError)]
        public async Task FindContentModelCatalogRelationsByCodesAsync_Test_WithErrorStatusCode(HttpStatusCode statusCode)
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "withoutGroupName"
            };
            var catalogUID = "1e077ba6-f8d5-e711-8172-0e8e465e7726";
            var json = "[]";

            var httpMessageHandlerMock = SetupMockedHttpMessageHandler(
                json,
                RouteSuffixFindContentModelCatalogRelationsByCodes,
                statusCode);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost:1234");
            var client = new CdcApiClient(httpClient);

            //Act
            Func<Task> action = async () => await client.FindContentModelCatalogRelationsByCodesAsync(
                ContentModel,
                catalogUID,
                cvxCodes);

            //Assert
            await action.Should().ThrowAsync<ApiException>();
        }

        [Test]
        public async Task FindContentModelCatalogRelationsByCodesAsync_Test()
        {
            //Arrange
            var cvxCodes = new string[]
            {
                "110", "withoutGroupName"
            };
            var catalogUID = "1e077ba6-f8d5-e711-8172-0e8e465e7726";
            var json = "[{\"RequestedTerm\":{\"CatalogIdentifier\":\"1e077ba6-f8d5-e711-8172-0e8e465e7726\",\"TermSourceCode\":\"110\",\"IncludeDomainCharacteristics\":false},\"Messages\":[],\"RelatedItems\":[{\"RelationshipUID\":\"53b4edf4-d05e-429c-8d78-f6463ce128d1\",\"RelationshipMnemonic\":\"IS_MEMBER_OF\",\"CatalogUID\":\"19eef0b1-fcd5-e711-9af6-001c422a1053\",\"CatalogMnemonic\":\"CDC_IMMUN_VAC_GRP\",\"RelatedTerm\":{\"TermDescription\":\"DTAP\"}}],\"SharedRelations\":[]}]";

            var httpMessageHandlerMock = SetupMockedHttpMessageHandler(
                json,
                RouteSuffixFindContentModelCatalogRelationsByCodes);
            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost:1234");
            var client = new CdcApiClient(httpClient);

            //Act
            var result = await client.FindContentModelCatalogRelationsByCodesAsync(
                ContentModel,
                catalogUID,
                cvxCodes);
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
        }

        private Mock<HttpMessageHandler> SetupMockedHttpMessageHandler(
            string json,
            string requestUriSegment,
            HttpStatusCode? statusCode = null)
        {
            var httpResponse = new HttpResponseMessage();
            httpResponse.StatusCode = statusCode ?? HttpStatusCode.OK;
            httpResponse.Content = new StringContent(json, Encoding.UTF8, "application/json");

            Mock<HttpMessageHandler> httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(
                        r => r.Method == HttpMethod.Get
                             && r.RequestUri.ToString().Contains(requestUriSegment)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            return httpMessageHandlerMock;
        }
    }
}
// <copyright file="ProvidersControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Controllers;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    [SetCulture("en-us")]
    public class ProvidersControllerTests
    {
        private Mock<ILogger<ProvidersController>> _loggerMock;
        private Mock<IProviderService> _serviceMock;
        private ProvidersController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<ProvidersController>>();
            _serviceMock = new Mock<IProviderService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new ProvidersController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        [Test]
        public async Task GetProvidersByLatLngTest()
        {
            //Arrange
            const string lat = "test-lat";
            const string lng = "test-lng";
            const string providerType = "test-provider-type";

            var providers = SetupService(lat, lng, providerType);
            _controller.MockObjectValidator();

            //Act
            var actionResult = await _controller.GetProvidersByLatLng(lat, lng, providerType);

            //Assert
            _serviceMock.Verify(
               x => x.GetProvidersByLatLngAsync(lat, lng, providerType),
               Times.Once());

            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<Provider>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(providers);
        }

        private IEnumerable<Provider> SetupService(string lat, string lng, string providerType)
        {
            IEnumerable<Provider> providers = new Provider[]
         {
                new()
         };
            _serviceMock
                .Setup(x => x.GetProvidersByLatLngAsync(lat, lng, providerType))
                .Returns(Task.FromResult(providers));

            return providers;
        }
    }
}

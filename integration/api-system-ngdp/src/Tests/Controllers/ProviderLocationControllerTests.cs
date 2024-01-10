// <copyright file="ProviderLocationControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Common.Testing;
using Duly.Ngdp.Api.Controllers;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Tests.Controllers
{
    [TestFixture]
    public class ProviderLocationControllerTests
    {
        private Mock<IProviderRepository> _providerRepositoryMocked;
        private Mock<ILogger<ProviderLocationController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]

        public void SetUp()
        {
            _providerRepositoryMocked = new Mock<IProviderRepository>();
            _loggerMocked = new Mock<ILogger<ProviderLocationController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetProvidersByLatLng_Test()
        {
            //Arrange
            const string lat = "28.535517";
            const string lng = "77.391029";
            const string providerType = "Gastroenterology";

            var controller = new ProviderLocationController( _loggerMocked.Object, _iWebHostEnvironment.Object, _providerRepositoryMocked.Object);
            controller.MockObjectValidator();

            var provider = SetupRepository(lat, lng, providerType);

            //Act
            var actionResult = await controller.GetProvidersByLatLng(lat, lng, providerType);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<ProviderLocation>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(provider);
        }

        private IEnumerable<ProviderLocation> SetupRepository(string lat, string lng, string providerType)
        {
            IEnumerable<ProviderLocation> providers = new ProviderLocation[]
           {
                new()
           };

            _providerRepositoryMocked
                 .Setup(repository =>
                    repository.GetProvidersByLatLng(lat, lng, providerType))
                .ReturnsAsync(providers);

            return providers;
        }
    }
}

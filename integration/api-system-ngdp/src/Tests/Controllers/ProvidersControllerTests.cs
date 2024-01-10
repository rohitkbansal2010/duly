// <copyright file="ProvidersControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

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
    public class ProvidersControllerTests
    {
        private Mock<IRecommendedProviderRepository> _recommendedProvidersRepositoryMocked;
        private Mock<ILogger<ProvidersController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _recommendedProvidersRepositoryMocked = new Mock<IRecommendedProviderRepository>();
            _loggerMocked = new Mock<ILogger<ProvidersController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetAppointmentsForSpecificLocationTest()
        {
            //Arrange
            const string referralId = "test-ref-id";
            var providers = SetupRepository(referralId);

            var controller = new ProvidersController(_loggerMocked.Object, _iWebHostEnvironment.Object, _recommendedProvidersRepositoryMocked.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.GetRecommendedProvidersByReferralId(referralId);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<RecommendedProvider>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(providers);
        }

        private IEnumerable<RecommendedProvider> SetupRepository(string referralId)
        {
            IEnumerable<RecommendedProvider> providers = new RecommendedProvider[]
            {
                new()
            };

            _recommendedProvidersRepositoryMocked
                .Setup(repository =>
                    repository.GetRecommendedProvidersByReferralIdAsync(referralId))
                .ReturnsAsync(providers);

            return providers;
        }
    }
}

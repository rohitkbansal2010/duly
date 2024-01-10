// <copyright file="ProviderServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Extensions;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class ProviderServiceTests
    {
        private Mock<IProviderRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IProviderRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetProvidersByLatLngAsyncTest()
        {
            //Arrange
            const string lat = "41.762";
            const string lng = "-88.1522";
            const string providerType = "GASTROENTEROLOGY";
            var locations = BuildProviders(lat, lng, providerType);
            ConfigureMapper(locations);
            var serviceMock = new ProviderService(
                _mapperMock.Object,
                _repositoryMock.Object);

            //Act
            var result = await serviceMock.GetProvidersByLatLngAsync(lat, lng, providerType);

            //Assert
            _repositoryMock.Verify(x => x.GetProvidersByLatLngAsync(lat, lng, providerType), Times.Once());

            result.Should().NotBeNull();
        }

        private IEnumerable<Provider> BuildProviders(string lat, string lng, string providerType)
        {
            IEnumerable<Provider> providers = new Provider[]
                 {
                            new()
                 };

            _repositoryMock
                .Setup(repo => repo.GetProvidersByLatLngAsync(lat, lng, providerType))
                .Returns(Task.FromResult(providers));

            return providers;
        }

        private void ConfigureMapper(IEnumerable<Provider> locations)
        {
            ICollection<Provider> providers = new Provider[]
                {
                                new()
                };

            _mapperMock
                .Setup(mapper => mapper.Map<ICollection<Provider>>(locations))
                .Returns(providers);
        }
    }
}
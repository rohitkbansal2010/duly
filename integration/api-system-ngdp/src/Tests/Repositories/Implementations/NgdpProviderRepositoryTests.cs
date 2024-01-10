// <copyright file="NgdpProviderRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Ngdp.Adapter.Adapters.Interfaces;
using Duly.Ngdp.Api.Repositories.Implementations;
using Duly.Ngdp.Contracts;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdapterModels = Duly.Ngdp.Adapter.Adapters.Models;

namespace Duly.Ngdp.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class NgdpProviderRepositoryTests
    {
        private Mock<IMapper> _mapperMocked;
        private Mock<IProviderAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IProviderAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]

        public async Task GetProvidersByLatLngAsync_Test()
        {
            //Arrange
            const string lat = "28.535517";
            const string lng = "77.391029";
            const string providerType = "Gastroenterology";

            var provider = SetupAdapter(lat, lng, providerType);
            var providerLocation = SetupMapper(provider);

            var repository = new NgdpProviderRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var result = await repository
                .GetProvidersByLatLng(lat, lng, providerType);

            //Assert
            result.Should().BeEquivalentTo(providerLocation);
        }

        private IEnumerable<AdapterModels.ProviderLocation> SetupAdapter(string lat, string lng, string providerType)
        {
            IEnumerable<AdapterModels.ProviderLocation> providers = new AdapterModels.ProviderLocation[]
            {
                new()
            };

            _adapterMocked
                .Setup(adapter => adapter.FindProviderByLatLngAsync(
                    It.Is<AdapterModels.ProviderSearchCriteria>(criteria =>
                    criteria.Lat == lat
                    && criteria.Lng == lng
                    && criteria.ProviderType == providerType)))
                .ReturnsAsync(providers);

            return providers;
        }

        private IEnumerable<ProviderLocation> SetupMapper(IEnumerable<AdapterModels.ProviderLocation> provider)
        {
            IEnumerable<ProviderLocation> systemLocations = new ProviderLocation[]
            {
                new()
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<ProviderLocation>>(provider))
                .Returns(systemLocations);

            return systemLocations;
        }
    }
}

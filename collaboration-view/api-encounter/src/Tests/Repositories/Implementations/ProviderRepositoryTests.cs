// -----------------------------------------------------------------------
// <copyright file="ProviderRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.Ngdp.Api.Client;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class ProviderRepositoryTests
    {
        //private const string Lat = "test-lat";
        //private const string Lng = "test-lng";
        //private const string ProviderType = "test-provider-type";

        private Mock<IEncounterContext> _encounterContextMocked;
        private Mock<ILatlngClient> _clientMocked;
        private Mock<IMapper> _mapperMocked;
        private Mock<IClient> _clientMockedDetails;

        [SetUp]
        public void SetUp()
        {
            _encounterContextMocked = new Mock<IEncounterContext>();
            _mapperMocked = new Mock<IMapper>();
            _clientMocked = new Mock<ILatlngClient>();
            _clientMockedDetails = new Mock<IClient>();
        }

        [Test]

        public async Task GetProvidersByLatLngAsyncTest()
        {
            //Arrange
            const string lat = "41.762";
            const string lng = "-88.1522";
            const string providerType = "GASTROENTEROLOGY";

            var locations = BuildLocations(lat, lng, providerType);
            ConfigureMapper(locations);
            var _repositoryMocked = new ProviderRepository(
               _encounterContextMocked.Object,
               _clientMockedDetails.Object,
               _clientMocked.Object,
               _mapperMocked.Object);

            //Act
            var results = await _repositoryMocked.GetProvidersByLatLngAsync(lat, lng, providerType);

            //Assert
            _clientMocked.Verify(x => x.ProviderLocationAsync(lat, lng, providerType, It.IsAny<Guid>(), default), Times.Once());
            _mapperMocked.Verify(x => x.Map<IEnumerable<Models.Provider>>(locations), Times.Once());

            //results.Should().NotBeNullOrEmpty();
        }

        private ICollection<ProviderLocation> BuildLocations(string lat, string lng, string providerType)
        {
            ICollection<ProviderLocation> providers = new ProviderLocation[]
            {
                new()
            };

            _clientMocked
               .Setup(client => client.ProviderLocationAsync(lat, lng, providerType, It.IsAny<Guid>(), default))
               .Returns(Task.FromResult(providers));

            return providers;
        }

        private void ConfigureMapper(ICollection<ProviderLocation> locations)
        {
            ICollection<Models.Provider> providers = new Models.Provider[]
      {
                new()
      };

            _mapperMocked
                .Setup(mapper => mapper.Map<ICollection<Models.Provider>>(locations))
                .Returns(providers);
        }
    }
}

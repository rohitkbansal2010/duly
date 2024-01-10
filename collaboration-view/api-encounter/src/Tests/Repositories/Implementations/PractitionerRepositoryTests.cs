// <copyright file="PractitionerRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
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
    public class PractitionerRepositoryTests
    {
        private Mock<IEncounterContext> _encounterContextMocked;
        private Mock<ISitesClient> _sitesClientMocked;
        private Mock<IClient> _clientMocked;
        private Mock<IMapper> _mapperMocked;

        [SetUp]
        public void SetUp()
        {
            ConfigureEncounterContext();
            _sitesClientMocked = new Mock<ISitesClient>();
            _clientMocked = new Mock<IClient>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task GetPractitionersBySiteIdAsyncTest()
        {
            //Arrange
            const string siteId = "123";
            var epicPractitioners = BuildEpicPractitioners(siteId);
            ConfigureMapper(epicPractitioners);
            IPractitionerRepository repositoryMocked = new PractitionerRepository(
                _encounterContextMocked.Object,
                _mapperMocked.Object,
                _sitesClientMocked.Object,
                _clientMocked.Object);

            //Act
            var results = await repositoryMocked.GetPractitionersBySiteIdAsync(siteId);

            //Assert
            _sitesClientMocked.Verify(x => x.PractitionersAsync(siteId, It.IsAny<Guid>(), default), Times.Once());

            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(epicPractitioners.Count);
            results.First().Id.Should().Be(epicPractitioners.First().Id);
        }

        [Test]
        public async Task GetPractitionersByIdsAsyncTest()
        {
            //Arrange
            var practitionerIds = new[] { "123" };
            var epicPractitioners = BuildEpicPractitionersByIds(practitionerIds);
            ConfigureMapper(epicPractitioners);

            IPractitionerRepository repositoryMocked = new PractitionerRepository(
                _encounterContextMocked.Object,
                _mapperMocked.Object,
                _sitesClientMocked.Object,
                _clientMocked.Object);

            //Act
            var results = await repositoryMocked.GetPractitionersByIdsAsync(practitionerIds);

            //Assert
            _clientMocked.Verify(x => x.PractitionersAsync(practitionerIds, It.IsAny<Guid>(), default), Times.Once());

            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(practitionerIds.Length);
            results.First().Id.Should().Be(practitionerIds.First());
        }

        [Test]
        public async Task GetPractitionersByIdsAsync_ReturnEmpty_Test()
        {
            //Arrange
            var practitionerIds = Array.Empty<string>();

            IPractitionerRepository repositoryMocked = new PractitionerRepository(
                _encounterContextMocked.Object,
                _mapperMocked.Object,
                _sitesClientMocked.Object,
                _clientMocked.Object);

            //Act
            var results = await repositoryMocked.GetPractitionersByIdsAsync(practitionerIds);

            //Assert
            _clientMocked.VerifyNoOtherCalls();
            results.Should().BeEmpty();
        }

        private void ConfigureEncounterContext()
        {
            _encounterContextMocked = new Mock<IEncounterContext>();

            _encounterContextMocked
                .Setup(ctx => ctx.GetXCorrelationId())
                .Returns(Guid.NewGuid());
        }

        private ICollection<PractitionerGeneralInfo> BuildEpicPractitioners(string siteId)
        {
            ICollection<PractitionerGeneralInfo> practitioners = new PractitionerGeneralInfo[]
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _sitesClientMocked
                .Setup(client => client.PractitionersAsync(siteId, It.IsAny<Guid>(), default))
                .Returns(Task.FromResult(practitioners));

            return practitioners;
        }

        private ICollection<PractitionerGeneralInfo> BuildEpicPractitionersByIds(string[] practitionerIds)
        {
            ICollection<PractitionerGeneralInfo> practitioners = practitionerIds.Select(p =>
                    new PractitionerGeneralInfo
                    {
                        Id = p
                    })
                .ToArray();

            _clientMocked
                .Setup(client => client.PractitionersAsync(practitionerIds, It.IsAny<Guid>(), default))
                .Returns(Task.FromResult(practitioners));

            return practitioners;
        }

        private void ConfigureMapper(IEnumerable<PractitionerGeneralInfo> epicPractitioners)
        {
            IEnumerable<Models.PractitionerGeneralInfo> practitioners = new Models.PractitionerGeneralInfo[]
            {
                new ()
                {
                    Id = epicPractitioners.First().Id
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Models.PractitionerGeneralInfo>>(epicPractitioners))
                .Returns(practitioners);
        }
    }
}

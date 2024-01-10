// <copyright file="PractitionerServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using Duly.Common.Security.Entities;
using Duly.Common.Security.Interfaces;
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
    public class PractitionerServiceTests
    {
        private Mock<IPractitionerRepository> _repositoryMocked;
        private Mock<IMapper> _mapperMocked;
        private Mock<IActiveDirectoryAccountIdentityService> _activeDirectoryAccountIdentityMocked;

        [SetUp]
        public void SetUp()
        {
            _repositoryMocked = new Mock<IPractitionerRepository>();
            _mapperMocked = new Mock<IMapper>();
            _activeDirectoryAccountIdentityMocked = new Mock<IActiveDirectoryAccountIdentityService>();
        }

        [Test]
        public async Task GetPractitionersBySiteIdAsyncTest()
        {
            //Arrange
            const string siteId = "123";
            var modelActiveUser = BuildModelActiveUser();
            var modelsPractitioners = BuildModelsPractitioners(siteId);
            ConfigureMapper(modelsPractitioners);
            var serviceMocked = new PractitionerService(
                _mapperMocked.Object,
                _repositoryMocked.Object,
                _activeDirectoryAccountIdentityMocked.Object);

            //Act
            var results = await serviceMocked.GetPractitionersBySiteIdAsync(siteId);

            //Assert
            _repositoryMocked.Verify(x => x.GetPractitionersBySiteIdAsync(siteId), Times.Once());

            results = results.ToArray();
            results.Should().NotBeNullOrEmpty();
            results.Should().AllBeOfType<ApiContracts.PractitionerGeneralInfo>();
            results.Should().HaveCount(modelsPractitioners.Count());
            results.First().Id.Should().Be(modelsPractitioners.First().Id);
            results.First().HumanName.FamilyName.Should().Be(modelActiveUser.LastName);
            results.First().HumanName.GivenNames.First().Should().Be(modelActiveUser.FirstName);
        }

        [Test]
        public async Task GetPractitionersBySiteIdAsync_NotMatched_Test()
        {
            //Arrange
            const string siteId = "123";
            var modelActiveUser = BuildModelActiveUser(siteId);
            var modelsPractitioners = BuildModelsPractitioners(siteId);
            ConfigureMapper(modelsPractitioners);
            var serviceMocked = new PractitionerService(
                _mapperMocked.Object,
                _repositoryMocked.Object,
                _activeDirectoryAccountIdentityMocked.Object);

            //Act
            var results = await serviceMocked.GetPractitionersBySiteIdAsync(siteId);

            //Assert
            _repositoryMocked.Verify(x => x.GetPractitionersBySiteIdAsync(siteId), Times.Once());

            results = results.ToArray();
            results.Should().NotBeNullOrEmpty();
            results.Should().AllBeOfType<ApiContracts.PractitionerGeneralInfo>();
            results.Should().HaveCount(modelsPractitioners.Count() + 1);
            results.First().Id.Should().Be(modelActiveUser.Id);
            results.First().HumanName.FamilyName.Should().Be(modelActiveUser.LastName);
            results.First().HumanName.GivenNames.First().Should().Be(modelActiveUser.FirstName);
        }

        private AzureActiveDirectoryUserAccountIdentity BuildModelActiveUser(string lastName = "Test")
        {
            var id = Guid.NewGuid().ToString();
            var activeUser = new AzureActiveDirectoryUserAccountIdentity
            {
                Id = id,
                LastName = lastName,
                FirstName = "Bob"
            };

            _activeDirectoryAccountIdentityMocked
                .Setup(service => service.GetUserAsync())
                .Returns(() => Task.FromResult(activeUser));

            _mapperMocked
                .Setup(mapper => mapper.Map<ApiContracts.PractitionerGeneralInfo>(activeUser))
                .Returns(new ApiContracts.PractitionerGeneralInfo
                {
                    Id = id,
                    HumanName = new Contracts.HumanName
                    {
                        FamilyName = lastName,
                        GivenNames = new[] { "Bob" }
                    }
                });

            return activeUser;
        }

        private IEnumerable<PractitionerGeneralInfo> BuildModelsPractitioners(string siteId)
        {
            IEnumerable<PractitionerGeneralInfo> practitioners = new PractitionerGeneralInfo[]
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _repositoryMocked
                .Setup(repo => repo.GetPractitionersBySiteIdAsync(siteId))
                .Returns(Task.FromResult(practitioners));

            return practitioners;
        }

        private void ConfigureMapper(IEnumerable<PractitionerGeneralInfo> modelsPractitioners)
        {
            IEnumerable<ApiContracts.PractitionerGeneralInfo> practitioners = new ApiContracts.PractitionerGeneralInfo[]
            {
                new ()
                {
                    Id = modelsPractitioners.First().Id,
                    HumanName = new ApiContracts.HumanName
                    {
                        FamilyName = "Test",
                        GivenNames = new[] { "Bob" }
                    }
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<ApiContracts.PractitionerGeneralInfo>>(modelsPractitioners))
                .Returns(practitioners);
        }
    }
}
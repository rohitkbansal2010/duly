// <copyright file="UserServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using Duly.Common.Security.Entities;
using Duly.Common.Security.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IActiveDirectoryAccountIdentityService> _activeDirectoryAccountIdentityMocked;

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();
            _activeDirectoryAccountIdentityMocked = new Mock<IActiveDirectoryAccountIdentityService>();
        }

        [Test]
        public async Task GetActiveUserAsyncTest()
        {
            //Arrange
            var modelActiveUser = BuildModelActiveUser();
            var serviceMock = new UserService(
                _mapperMock.Object,
                _activeDirectoryAccountIdentityMocked.Object);

            //Act
            var results = await serviceMock.GetActiveUserAsync();

            //Assert
            results.Should().NotBeNull();
            results.Should().BeOfType<Contracts.User>();
            results.Id.Should().Be(modelActiveUser.Id);
            results.Name.FamilyName.Should().Be(modelActiveUser.LastName);
            results.Name.GivenNames.First().Should().Be(modelActiveUser.FirstName);
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

            _mapperMock
                .Setup(mapper => mapper.Map<Contracts.User>(activeUser))
                .Returns(new Contracts.User
                {
                    Id = id,
                    Name = new Contracts.HumanName
                    {
                        FamilyName = lastName,
                        GivenNames = new[] { "Bob" }
                    }
                });

            return activeUser;
        }
    }
}

// <copyright file="CustomActionsServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces.CarePlan;
using Duly.CollaborationView.Encounter.Api.Services.Implementations.CarePlan;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations.CarePlan
{
    [TestFixture]
    public class CustomActionsServiceTests
    {
        private const long CustomActionId = 4;
        private Mock<ICustomActionsRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<ICustomActionsRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task PostCustomActionsSuccessTests()
        {
            var _customActions = new CustomActions
            {
                ActionName = "Test action name",
                Description = "dummy description for test"
            };

            var _mappedCustomActions = new Api.Repositories.Models.CarePlan.CustomActions
            {
            ActionName = _customActions.ActionName,
            Description = _customActions.Description
            };

            _repositoryMock
              .Setup(repo => repo.PostCustomActionsAsync(_mappedCustomActions))
              .Returns(Task.FromResult(CustomActionId));

            _mapperMock
            .Setup(mapper => mapper.Map<Api.Repositories.Models.CarePlan.CustomActions>(_customActions))
            .Returns(_mappedCustomActions);

            var serviceMock = new CustomActionsService(_mapperMock.Object, _repositoryMock.Object);

            var results = await serviceMock.PostCustomActionsAsync(_customActions);

            //Assert
            _repositoryMock.Verify(x => x.PostCustomActionsAsync(_mappedCustomActions), Times.Once());

            results.Should().NotBe(0);
        }
    }
}
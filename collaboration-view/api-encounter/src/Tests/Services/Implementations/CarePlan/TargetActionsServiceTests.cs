// <copyright file="TargetActionsServiceTests.cs" company="Duly Health and Care">
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
    public class TargetActionsServiceTests
    {
        private Mock<ITargetActionsRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<ITargetActionsRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        public async Task GetTargetActionsSuccessTests()
        {
            //Arrange
            IEnumerable<Api.Repositories.Models.CarePlan.TargetActions> targetActions = new Api.Repositories.Models.CarePlan.TargetActions[]
            {
                 new()
                 {
                     ActionId = 1,
                     ActionName = "TestActionName",
                     Description = "TestDescription"
                 }
            };

            IEnumerable<Contracts.CarePlan.TargetActions> mappedTargetActions = new Contracts.CarePlan.TargetActions[]
            {
                  new()
                  {
                     ActionId = 1,
                     ActionName = "TestActionName",
                     Description = "TestDescription"
                  }
            };

            List<TargetActions> listOfConditions = new List<TargetActions>();

            _repositoryMock
              .Setup(repo => repo.GetTargetActionsByConditionIdAndTargetIdAsync(1, 1))
              .Returns(Task.FromResult(targetActions));

            _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<Contracts.CarePlan.TargetActions>>(targetActions))
            .Returns(mappedTargetActions);

            var serviceMock = new TargetActionsService(_mapperMock.Object, _repositoryMock.Object);

            //Act
            //var results = await serviceMock.GetConditions();
            var results = await serviceMock.GetTargetActionsByConditionIdAndTargetIdAsync(1, 1);

            //Assert
            _repositoryMock.Verify(x => x.GetTargetActionsByConditionIdAndTargetIdAsync(1, 1), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<List<TargetActions>>();
        }
    }
}

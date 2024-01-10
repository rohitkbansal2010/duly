// <copyright file="TargetActionsRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Implementations.CarePlan;
using Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan;
using Duly.Common.DataAccess.Contexts.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations.CarePlan
{
    [TestFixture]
    public class TargetActionsRepositoryTests
    {
        private const string GetTargetActionsByTargetIdProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetTargetActionsByConditionIdAndTargetId]";
        private Mock<IDapperContext> _dapperMock;

        [SetUp]
        public void SetUp()
        {
            _dapperMock = new Mock<IDapperContext>();
        }

        [Test]
        public async Task GetTargetActionsSuccessTest()
        {
            //Arrange
            var repositoryMock = new TargetActionsRepository(_dapperMock.Object);
            var TargetActions = SetupDapperContext();

            //Act
            var result = await repositoryMock.GetTargetActionsByConditionIdAndTargetIdAsync(1, 1);

            //Assert
            result.Should().BeEquivalentTo(TargetActions);
        }

        private IEnumerable<TargetActions> SetupDapperContext()
        {
            IEnumerable<TargetActions> targetActions = new TargetActions[]
            {
                new()
            };

            _dapperMock
                .Setup(context =>
                context.QueryAsync<TargetActions>(GetTargetActionsByTargetIdProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(targetActions);

            return targetActions;
        }
    }
}

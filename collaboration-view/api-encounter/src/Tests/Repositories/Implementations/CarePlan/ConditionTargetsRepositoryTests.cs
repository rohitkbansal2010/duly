// <copyright file="ConditionTargetsRepositoryTests.cs" company="Duly Health and Care">
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
    public class ConditionTargetsRepositoryTests
    {
        private const string GetAllConditionTargetsByConditionIdsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetAllConditionTargetsByConditionIds]";
        private readonly string _conditionIds = "1";
        private Mock<IDapperContext> _dapperMock;

        [SetUp]
        public void SetUp()
        {
            _dapperMock = new Mock<IDapperContext>();
        }

        [Test]
        public async Task GetConditionTargetsByConditionIdSuccessTest()
        {
            //Arrange
            var repositoryMock = new ConditionTargetsRepository(_dapperMock.Object);
            var conditionTargets = SetupDapperContext();

            //Act
            var result = await repositoryMock.GetConditionTargetsByConditionId(_conditionIds);

            //Assert
            result.Should().BeEquivalentTo(conditionTargets);
        }

        private IEnumerable<ConditionTargets> SetupDapperContext()
        {
            IEnumerable<ConditionTargets> conditionTargets = new ConditionTargets[]
            {
                new()
            };

            _dapperMock
                .Setup(context =>
                context.QueryAsync<ConditionTargets>(GetAllConditionTargetsByConditionIdsProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(conditionTargets);

            return conditionTargets;
        }
    }
}
// <copyright file="ConditionRepositoryTests.cs" company="Duly Health and Care">
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
    public class ConditionRepositoryTests
    {
        private const string GetAllActiveConditionsProcedureName = Constants.CarePlanSchemaName + Constants.NameSeparator + "[uspGetAllActiveConditions]";
        private Mock<IDapperContext> _dapperMock;

        [SetUp]
        public void SetUp()
        {
            _dapperMock = new Mock<IDapperContext>();
        }

        [Test]
        public async Task GetConditionsSuccessTest()
        {
            //Arrange
            var repositoryMock = new ConditionRepository(_dapperMock.Object);
            var conditionTargets = SetupDapperContext();

            //Act
            var result = await repositoryMock.GetConditions();

            //Assert
            result.Should().BeEquivalentTo(conditionTargets);
        }

        private IEnumerable<Condition> SetupDapperContext()
        {
            IEnumerable<Condition> conditionTargets = new Condition[]
            {
                new()
            };

            _dapperMock
                .Setup(context =>
                context.QueryAsync<Condition>(GetAllActiveConditionsProcedureName, It.IsAny<object>(), default, default))
                .ReturnsAsync(conditionTargets);

            return conditionTargets;
        }
    }
}
// <copyright file="ConditionServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class ConditionServiceTests
    {
        private Mock<IConditionRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IConditionRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetHealthConditionsByPatientIdAsyncTest()
        {
            //Arrange
            var patientId = Guid.NewGuid().ToString();
            ConditionClinicalStatus[] conditionStatuses =
            {
                ConditionClinicalStatus.Active,
                ConditionClinicalStatus.Inactive,
                ConditionClinicalStatus.Resolved
            };

            var conditions = BuildData(patientId, conditionStatuses);
            ConfigureMapper(conditions);
            var serviceMock = new ConditionService(
                _mapperMock.Object,
                _repositoryMock.Object);

            //Act
            var result = await serviceMock.GetHealthConditionsByPatientIdAsync(patientId);

            //Assert
            _repositoryMock.Verify(x => x.GetConditionsForPatientAsync(patientId, conditionStatuses), Times.Once());

            result.Should().NotBeNull();
            result.Should().BeOfType<HealthConditions>();

            result.CurrentHealthConditions.Should().NotBeNullOrEmpty();
            result.CurrentHealthConditions.Should().HaveCount(1);
            result.CurrentHealthConditions.Should().BeOfType<HealthCondition[]>();
            result.CurrentHealthConditions.First().Title.Should().Be("test0");

            result.PreviousHealthConditions.Should().NotBeNullOrEmpty();
            result.PreviousHealthConditions.Should().HaveCount(2);
            result.PreviousHealthConditions.Should().BeOfType<HealthCondition[]>();
            result.PreviousHealthConditions.First().Title.Should().Be("test1");
            result.PreviousHealthConditions.Last().Title.Should().Be("test2");
        }

        private IEnumerable<Condition> BuildData(string patientId, ConditionClinicalStatus[] conditionStatuses)
        {
            IEnumerable<Condition> conditions = new List<Condition>
            {
                new()
                {
                    ClinicalStatus = ConditionClinicalStatus.Active,
                    Name = "test0"
                },
                new()
                {
                    ClinicalStatus = ConditionClinicalStatus.Resolved,
                    Name = "test1"
                },
                new()
                {
                    ClinicalStatus = ConditionClinicalStatus.Inactive,
                    Name = "test2"
                },
            };

            _repositoryMock
                .Setup(repo => repo.GetConditionsForPatientAsync(patientId, conditionStatuses))
                .Returns(Task.FromResult(conditions));

            return conditions;
        }

        private void ConfigureMapper(IEnumerable<Condition> conditions)
        {
            var modelConditionsArray = conditions.ToArray();

            IEnumerable<HealthCondition> apiCurrentConditions = new List<HealthCondition>
            {
                new() { Title = modelConditionsArray[0].Name },
            };
            IEnumerable<HealthCondition> apiPreviousConditions = new List<HealthCondition>
            {
                new() { Title = modelConditionsArray[1].Name },
                new() { Title = modelConditionsArray[2].Name },
            };

            var modelCurrentConditions = modelConditionsArray.Where(x =>
                x.ClinicalStatus == ConditionClinicalStatus.Active);
            var modelPreviousConditions = modelConditionsArray.Where(x =>
                x.ClinicalStatus is ConditionClinicalStatus.Inactive or ConditionClinicalStatus.Resolved);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<HealthCondition>>(modelCurrentConditions))
                .Returns(apiCurrentConditions);
            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<HealthCondition>>(modelPreviousConditions))
                .Returns(apiPreviousConditions);
        }
    }
}

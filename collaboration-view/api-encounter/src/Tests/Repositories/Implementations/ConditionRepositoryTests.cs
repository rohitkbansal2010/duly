// <copyright file="ConditionRepositoryTests.cs" company="Duly Health and Care">
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
    public class ConditionRepositoryTests
    {
        private Mock<IEncounterContext> _encounterContextMock;
        private Mock<IConditionsClient> _clientMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            ConfigureEncounterContext();
            _clientMock = new Mock<IConditionsClient>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetConditionsForPatientAsyncTest()
        {
            //Arrange
            var patientId = Guid.NewGuid().ToString();
            ConditionClinicalStatus[] epicConditionStatuses =
            {
                ConditionClinicalStatus.Active,
                ConditionClinicalStatus.Inactive,
                ConditionClinicalStatus.Resolved
            };
            Models.ConditionClinicalStatus[] conditionStatuses =
            {
                Models.ConditionClinicalStatus.Active,
                Models.ConditionClinicalStatus.Inactive,
                Models.ConditionClinicalStatus.Resolved
            };
            var epicConditions = BuildEpicData(patientId, epicConditionStatuses);
            ConfigureMapper(epicConditions, epicConditionStatuses, conditionStatuses);
            IConditionRepository repositoryMock = new ConditionRepository(
                _encounterContextMock.Object,
                _mapperMock.Object,
                _clientMock.Object);

            //Act
            var results = await repositoryMock.GetConditionsForPatientAsync(patientId, conditionStatuses);

            //Assert
            _mapperMock.Verify(
                x => x.Map<ConditionClinicalStatus[]>(conditionStatuses),
                Times.Once());
            _clientMock.Verify(
                x => x.ProblemsAsync(patientId, epicConditionStatuses, It.IsAny<Guid>(), default),
                Times.Once());
            _mapperMock.Verify(
                x => x.Map<IEnumerable<Models.Condition>>(epicConditions),
                Times.Once());

            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(epicConditions.Count);
            results.First().Id.Should().Be(epicConditions.First().Id);
        }

        private void ConfigureEncounterContext()
        {
            _encounterContextMock = new Mock<IEncounterContext>();

            _encounterContextMock
                .Setup(ctx => ctx.GetXCorrelationId())
                .Returns(Guid.NewGuid());
        }

        private ICollection<Condition> BuildEpicData(string patientId, ConditionClinicalStatus[] epicConditionStatuses)
        {
            ICollection<Condition> conditions = new Condition[]
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString(),
                },
            };

            _clientMock
                .Setup(client => client.ProblemsAsync(patientId, epicConditionStatuses, It.IsAny<Guid>(), default))
                .Returns(Task.FromResult(conditions));

            return conditions;
        }

        private void ConfigureMapper(
            IEnumerable<Condition> epicConditions,
            ConditionClinicalStatus[] epicConditionStatuses,
            Models.ConditionClinicalStatus[] conditionStatuses)
        {
            IEnumerable<Models.Condition> conditions = new Models.Condition[]
            {
                new ()
                {
                    Id = epicConditions.First().Id,
                },
            };

            _mapperMock
                .Setup(mapper => mapper.Map<ConditionClinicalStatus[]>(conditionStatuses))
                .Returns(epicConditionStatuses);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<Models.Condition>>(epicConditions))
                .Returns(conditions);
        }
    }
}

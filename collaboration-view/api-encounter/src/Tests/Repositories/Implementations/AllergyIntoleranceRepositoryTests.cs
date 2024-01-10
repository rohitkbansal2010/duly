// <copyright file="AllergyIntoleranceRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllergyIntolerance = Duly.Clinic.Api.Client.AllergyIntolerance;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class AllergyIntoleranceRepositoryTests
    {
        private Mock<IEncounterContext> _encounterContextMock;
        private Mock<IAllergyIntoleranceClient> _clientMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            ConfigureEncounterContext();
            _clientMock = new Mock<IAllergyIntoleranceClient>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetAllergyIntolerancesForPatientAsyncTest()
        {
            //Arrange
            var patientId = Guid.NewGuid().ToString();
            var systemStatus = ClinicalStatus.Active;
            var systemAllergies = BuildAllergies(patientId, systemStatus);
            var status = AllergyIntoleranceClinicalStatus.Active;
            ConfigureMapper(systemAllergies, status);

            var repository = new AllergyIntoleranceRepository(
                _encounterContextMock.Object,
                _mapperMock.Object,
                _clientMock.Object);

            //Act
            var result = await repository.GetAllergyIntolerancesForPatientAsync(patientId, status);

            //Assert
            _mapperMock.Verify(
                x => x.Map<ClinicalStatus>(status),
                Times.Once());

            _clientMock.Verify(
                x => x.ConfirmedAsync(patientId, systemStatus, It.IsAny<Guid>(), default),
                Times.Once());

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(systemAllergies.Count);
            result.First().Id.Should().Be(systemAllergies.First().Id);
        }

        private void ConfigureEncounterContext()
        {
            _encounterContextMock = new Mock<IEncounterContext>();

            _encounterContextMock
                .Setup(ctx => ctx.GetXCorrelationId())
                .Returns(Guid.NewGuid());
        }

        private ICollection<AllergyIntolerance> BuildAllergies(string patientId, ClinicalStatus status)
        {
            ICollection<AllergyIntolerance> allergies = new AllergyIntolerance[]
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _clientMock
                .Setup(client => client.ConfirmedAsync(patientId, status, It.IsAny<Guid>(), default))
                .Returns(Task.FromResult(allergies));

            return allergies;
        }

        private void ConfigureMapper(ICollection<AllergyIntolerance> systemAllergies, AllergyIntoleranceClinicalStatus status)
        {
            IEnumerable<Models.AllergyIntolerance> allergies = new Models.AllergyIntolerance[]
            {
                new ()
                {
                    Id = systemAllergies.First().Id
                }
            };

            _mapperMock
                .Setup(mapper => mapper.Map<ClinicalStatus>(status))
                .Returns(ClinicalStatus.Active);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<Models.AllergyIntolerance>>(systemAllergies))
                .Returns(allergies);
        }
    }
}

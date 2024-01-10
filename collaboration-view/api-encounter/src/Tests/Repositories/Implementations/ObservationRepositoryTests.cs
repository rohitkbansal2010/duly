// <copyright file="ObservationRepositoryTests.cs" company="Duly Health and Care">
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

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class ObservationRepositoryTests
    {
        private Mock<IEncounterContext> _encounterContextMocked;
        private Mock<IVitalSignsClient> _clientVitalSignsMocked;
        private Mock<IObservationsClient> _clientObservationsMocked;
        private Mock<IMapper> _mapperMocked;

        [SetUp]
        public void Setup()
        {
            _encounterContextMocked = new Mock<IEncounterContext>();
            _clientVitalSignsMocked = new Mock<IVitalSignsClient>();
            _clientObservationsMocked = new Mock<IObservationsClient>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task GetLatestObservationsForPatientAsyncTest()
        {
            //Arrange
            const string patientId = "test";

            PrepareMockData(
                out var observationTypes,
                out var observationTypesApi,
                out var observations,
                out var observationsRepository);

            SetupMockedObjects(observationTypes, observationTypesApi, patientId, observations, observationsRepository);

            IObservationRepository _repositoryMocked = new ObservationRepository(
                _encounterContextMocked.Object,
                _clientVitalSignsMocked.Object,
                _clientObservationsMocked.Object,
                _mapperMocked.Object);

            //Act
            var result = await _repositoryMocked.GetLatestObservationsForPatientAsync(patientId, observationTypes);

            //Assert
            result.Should().NotBeEmpty();
            result.Count().Should().Be(1);
            result.First().Should().Be(observationsRepository[0]);
        }

        [Test]
        public async Task GetObservationsForPatientAsyncTest()
        {
            //Arrange
            const string patientId = "test";

            PrepareMockData(
                out var observationTypes,
                out var observationTypesApi,
                out var observations,
                out var observationsRepository);

            var lowerBoundDate = DateTime.Now.AddYears(-3);
            var upperBoundDate = DateTime.Now;

            SetupMockedObjects(patientId, observationTypes, observationTypesApi, lowerBoundDate, upperBoundDate, observations, observationsRepository);

            IObservationRepository _repositoryMocked = new ObservationRepository(
                _encounterContextMocked.Object,
                _clientVitalSignsMocked.Object,
                _clientObservationsMocked.Object,
                _mapperMocked.Object);

            //Act
            var result = await _repositoryMocked.GetObservationsForPatientAsync(patientId, lowerBoundDate, upperBoundDate, observationTypes);

            //Assert
            result.Should().NotBeEmpty();
            result.Count().Should().Be(1);
            result.First().Should().Be(observationsRepository[0]);
        }

        private static void PrepareMockData(
            out Api.Repositories.Models.ObservationType[] observationTypes,
            out ObservationType[] observationTypesApi,
            out Observation[] observations,
            out Api.Repositories.Models.Observation[] observationsRepository)
        {
            observationTypes = new[]
            {
                Api.Repositories.Models.ObservationType.OxygenSaturation
            };

            observationTypesApi = new[]
            {
                ObservationType.OxygenSaturation
            };

            observations = new Observation[]
            {
                new()
                {
                    Type = ObservationType.OxygenSaturation
                }
            };

            observationsRepository = new Api.Repositories.Models.Observation[]
            {
                new()
                {
                    Type = Api.Repositories.Models.ObservationType.OxygenSaturation
                }
            };
        }

        private void SetupMockedObjects(
            string patientId,
            Api.Repositories.Models.ObservationType[] observationTypes,
            ObservationType[] observationTypesApi,
            DateTime lowerBoundDate,
            DateTime upperBoundDate,
            Observation[] observations,
            Api.Repositories.Models.Observation[] observationsRepository)
        {
            _mapperMocked
                .Setup(mapper => mapper.Map<ObservationType[]>(observationTypes))
                .Returns(observationTypesApi);

            _clientObservationsMocked
                .Setup(client => client.VitalSignsAsync(patientId, observationTypesApi, lowerBoundDate, upperBoundDate, It.IsAny<Guid>(), default))
                .Returns(Task.FromResult((ICollection<Observation>)observations));

            _mapperMocked
                .Setup(mapper => mapper.Map<Api.Repositories.Models.Observation[]>(observations))
                .Returns(observationsRepository);
        }

        private void SetupMockedObjects(
            Api.Repositories.Models.ObservationType[] observationTypes,
            ObservationType[] observationTypesApi,
            string patientId,
            Observation[] observations,
            Api.Repositories.Models.Observation[] observationsRepository)
        {
            _mapperMocked
                .Setup(mapper => mapper.Map<ObservationType[]>(observationTypes))
                .Returns(observationTypesApi);

            _clientVitalSignsMocked
                .Setup(client => client.LastAsync(patientId, observationTypesApi, It.IsAny<Guid>(), default))
                .Returns(Task.FromResult((ICollection<Observation>)observations));

            _mapperMocked
                .Setup(mapper => mapper.Map<Api.Repositories.Models.Observation[]>(observations))
                .Returns(observationsRepository);
        }
    }
}

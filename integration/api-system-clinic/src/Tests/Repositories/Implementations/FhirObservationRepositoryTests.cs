// <copyright file="FhirObservationRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using AutoMapper;
using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class FhirObservationRepositoryTests
    {
        private Mock<IMapper> _mapperMocked;
        private Mock<IObservationEnricher> _enricherMocked;
        private Mock<IObservationWithCompartmentsAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IObservationWithCompartmentsAdapter>();
            _mapperMocked = new Mock<IMapper>();
            _enricherMocked = new Mock<IObservationEnricher>();
        }

        [Test]
        public async Task FindObservationsForPatientAsyncTest()
        {
            //Arrange
            const string patientId = "testId";
            var observationTypes = new[]
            {
                ObservationType.BloodPressure,
            };

            var lowerBoundDate = System.DateTime.Now;
            var upperBoundBoundDate = System.DateTime.Now;

            var observations = SetUpAdapter();
            var expectedObservations = SetUpMapper(observations);

            IObservationRepository repository = new FhirObservationRepository(_adapterMocked.Object, _mapperMocked.Object, _enricherMocked.Object);

            //Act
            var results = await repository.FindObservationsForPatientAsync(patientId, observationTypes, lowerBoundDate, upperBoundBoundDate);

            //Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(expectedObservations.Count());
            results.First().Type.Should().Be(expectedObservations.First().Type);
            results.First().Components.First().Type.Should().Be(expectedObservations.First().Components.First().Type);
            results.Last().Type.Should().Be(expectedObservations.Last().Type);
            results.Last().Components.First().Type.Should().Be(expectedObservations.Last().Components.First().Type);
        }

        [Test]
        public async Task FindLastObservationsForPatientAsyncTest()
        {
            //Arrange
            const string patientId = "testId";
            var observationTypes = new[]
            {
                ObservationType.BloodPressure,
            };
            var observations = SetUpAdapter();
            var expectedObservations = SetUpMapper(observations);
            IObservationRepository repository = new FhirObservationRepository(_adapterMocked.Object, _mapperMocked.Object, _enricherMocked.Object);

            //Act
            var results = await repository.FindLastObservationsForPatientAsync(patientId, observationTypes);

            //Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(1);
            results.First().Type.Should().Be(expectedObservations.Last().Type);
            results.First().Components.First().Type.Should().Be(expectedObservations.Last().Components.First().Type);
        }

        private IEnumerable<ObservationWithCompartments> SetUpAdapter()
        {
            IEnumerable<ObservationWithCompartments> observations = new ObservationWithCompartments[]
            {
                new()
                {
                    Resource = new R4.Observation()
                }
            };

            _adapterMocked
                .Setup(adapter => adapter.FindObservationsWithCompartmentsAsync(It.IsAny<ObservationSearchCriteria>()))
                .Returns(Task.FromResult(observations));

            return observations;
        }

        private IEnumerable<Observation> SetUpMapper(IEnumerable<ObservationWithCompartments> observations)
        {
            IEnumerable<Observation> observationsTarget = new Observation[]
            {
                new()
                {
                    Type = ObservationType.BloodPressure,
                    Date = new DateTimeOffset(2000, 1, 1, 5, 0, 0, default),
                    Components = new ObservationComponent[]
                    {
                        new()
                        {
                            Type = ObservationComponentType.Systolic,
                        }
                    },
                },
                new()
                {
                    Type = ObservationType.BloodPressure,
                    Date = new DateTimeOffset(2000, 1, 1, 10, 0, 0, default),
                    Components = new ObservationComponent[]
                    {
                        new()
                        {
                            Type = ObservationComponentType.Diastolic,
                        }
                    },
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Observation>>(observations, It.IsAny<Action<IMappingOperationOptions<object, IEnumerable<Observation>>>>()))
                .Returns(observationsTarget);
            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Observation>>(observations))
                .Returns(observationsTarget);

            return observationsTarget;
        }
    }
}

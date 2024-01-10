// <copyright file="ObservationWithCompartmentsAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Adapters.Implementations;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Exceptions;
using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;

using Hl7.Fhir.Rest;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class ObservationWithCompartmentsAdapterTests
    {
        private const string PatientId = "testId";

        private readonly Mock<IFhirClientR4> _mockedClient = new();
        private readonly Mock<IObservationWithCompartmentsBuilder> _mockedBuilder = new();

        [Test]
        public async Task FindObservationByIdAsync_MissingCattegory_Test()
        {
            //Arrange
            var components = SetUpClient();
            SetUpBuilder(components);

            IObservationWithCompartmentsAdapter adapter = new ObservationWithCompartmentsAdapter(_mockedClient.Object, _mockedBuilder.Object);
            var searchCriteria = new ObservationSearchCriteria { PatientId = PatientId };

            //Act
            Func<Task> action = async () => await adapter.FindObservationsWithCompartmentsAsync(searchCriteria);

            //Assert
            await action.Should().ThrowAsync<MandatoryQueryParameterMissingException>();
        }

        [Test]
        public async Task FindObservationByIdAsync_Success_Test()
        {
            //Arrange
            var components = SetUpClient();
            SetUpBuilder(components);

            IObservationWithCompartmentsAdapter adapter = new ObservationWithCompartmentsAdapter(_mockedClient.Object, _mockedBuilder.Object);
            var searchCriteria = new ObservationSearchCriteria { PatientId = PatientId, Category = "vital-signs" };

            //Act
            var result = await adapter.FindObservationsWithCompartmentsAsync(searchCriteria);

            //Assert
            result.Should().NotBeNull();
            result.First().Resource.Should().Be(components[0].Resource);
        }

        private R4.Bundle.EntryComponent[] SetUpClient()
        {
            var components = new R4.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new R4.Observation
                    {
                        Id = PatientId
                    }
                }
            };
            _mockedClient
                .Setup(client => client.FindObservationsAsync(It.IsAny<SearchParams>()))
                .Returns(
                    Task.FromResult(components));

            return components;
        }

        private void SetUpBuilder(IReadOnlyList<R4.Bundle.EntryComponent> components)
        {
            _mockedBuilder
                .Setup(builder => builder.ExtractObservations(components))
                .Returns(new ObservationWithCompartments[]
                    {
                        new()
                        {
                            Resource = (R4.Observation)components[0].Resource
                        }
                    });
        }
    }
}
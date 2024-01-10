// <copyright file="AllergyIntoleranceAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Adapters.Implementations;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Exceptions;
using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;

using Hl7.Fhir.Rest;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class AllergyIntoleranceAdapterTests
    {
        private readonly Mock<IFhirClientR4> _mockedClient = new();

        [Test]
        public async Task FindAllergyIntolerancesAsync_MissingPatientId_Test()
        {
            //Arrange
            var resources = SetUpClient();

            IAllergyIntoleranceAdapter adapter = new AllergyIntoleranceAdapter(_mockedClient.Object);
            var searchCriteria = new SearchCriteria();

            //Act
            Func<Task> action = async () => await adapter.FindAllergyIntolerancesAsync(searchCriteria);

            //Assert
            await action.Should().ThrowAsync<MandatoryQueryParameterMissingException>();
        }

        [Test]
        public async Task FindAllergyIntolerancesAsync_MissingStatus_Test()
        {
            //Arrange
            var resources = SetUpClient();

            IAllergyIntoleranceAdapter adapter = new AllergyIntoleranceAdapter(_mockedClient.Object);
            var searchCriteria = new SearchCriteria { PatientId = resources.First().Id };

            //Act
            Func<Task> action = async () => await adapter.FindAllergyIntolerancesAsync(searchCriteria);

            //Assert
            await action.Should().ThrowAsync<MandatoryQueryParameterMissingException>();
        }

        [Test]
        public async Task FindObservationByIdAsync_Success_Test()
        {
            //Arrange
            var resources = SetUpClient();

            IAllergyIntoleranceAdapter adapter = new AllergyIntoleranceAdapter(_mockedClient.Object);
            var searchCriteria = new SearchCriteria { PatientId = resources.First().Id, Status = "active" };

            //Act
            var result = await adapter.FindAllergyIntolerancesAsync(searchCriteria);

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(resources.Length);
        }

        private R4.AllergyIntolerance[] SetUpClient()
        {
            var resources = new R4.AllergyIntolerance[]
            {
                new()
                {
                    Id = Guid.NewGuid().ToString()
                },
                new()
                {
                    Id = Guid.NewGuid().ToString()
                },
                new()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };
            _mockedClient
                .Setup(client => client.FindResourcesAsync<R4.AllergyIntolerance>(It.IsAny<SearchParams>()))
                .Returns(
                    Task.FromResult(resources.AsEnumerable()));

            return resources;
        }
    }
}
// <copyright file="ConditionAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Adapters.Implementations;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
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
    public class ConditionAdapterTests
    {
        private readonly Mock<IFhirClientR4> _mockedClient = new();

        [Test]
        public async Task FindConditionsForPatientAsync_MissingPatientId_Test()
        {
            //Arrange
            var resources = SetUpClient();

            IHealthConditionAdapter adapter = new HealthConditionAdapter(_mockedClient.Object);
            var searchCriteria = new ConditionSearchCriteria { PatientId = "Some", Categories = new[] { "category" } };

            //Act
            Func<Task> action = async () => await adapter.FindConditionsForPatientAsync(searchCriteria);

            //Assert
            await action.Should().NotThrowAsync();
            _mockedClient.Verify(x => x.FindResourcesAsync<R4.Condition>(It.IsAny<SearchParams>()), Times.Once);
        }

        private R4.Condition[] SetUpClient()
        {
            var resources = new R4.Condition[]
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
                .Setup(client => client.FindResourcesAsync<R4.Condition>(It.IsAny<SearchParams>()))
                .Returns(
                    Task.FromResult(resources.AsEnumerable()));

            return resources;
        }
    }
}
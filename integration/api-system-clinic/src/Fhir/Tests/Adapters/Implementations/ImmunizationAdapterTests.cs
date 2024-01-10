// <copyright file="ImmunizationAdapterTests.cs" company="Duly Health and Care">
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class ImmunizationAdapterTests
    {
        [Test]
        public async Task FindImmunizationsForPatientAsyncTest()
        {
            //Arrange
            IEnumerable<R4.Immunization> immunizations = new R4.Immunization[]
            {
                new()
            };

            var mockedClient = new Mock<IFhirClientR4>();

            mockedClient
                .Setup(client => client.FindResourcesAsync<R4.Immunization>(It.IsAny<SearchParams>()))
                .ReturnsAsync(immunizations);

            var searchCriteria = new ImmunizationSearchCriteria
            {
                PatientId = "patientId"
            };

            IImmunizationAdapter adapter = new ImmunizationAdapter(mockedClient.Object);

            //Act
            var result = await adapter.FindImmunizationsForPatientAsync(searchCriteria);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Should().Be(immunizations.First());
        }
    }
}

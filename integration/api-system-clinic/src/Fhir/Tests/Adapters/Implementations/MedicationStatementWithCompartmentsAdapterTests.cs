// <copyright file="MedicationStatementWithCompartmentsAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias stu3;

using Duly.Clinic.Fhir.Adapter.Adapters.Implementations;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;
using Hl7.Fhir.Rest;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

using STU3 = stu3::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class MedicationStatementWithCompartmentsAdapterTests
    {
        private Mock<IFhirClientSTU3> _mockedClient;
        private Mock<IFhirClientR4> _r4mockedClient;
        private Mock<IMedicationStatementWithCompartmentsBuilder> _mockedBuilder;
        private Mock<IMedicationRequestWithCompartmentsBuilder> _r4mockedBuilder;
        private Mock<IPractitionerWithRolesBuilder> _mockPractitionerBuilder;

        [SetUp]
        public void SetUp()
        {
            _mockedClient = new Mock<IFhirClientSTU3>();
            _r4mockedClient = new Mock<IFhirClientR4>();
            _r4mockedBuilder = new Mock<IMedicationRequestWithCompartmentsBuilder>();
            _mockedBuilder = new Mock<IMedicationStatementWithCompartmentsBuilder>();
            _mockPractitionerBuilder = new Mock<IPractitionerWithRolesBuilder>();
        }

        [Test]
        public async Task FindMedicationWithCompartmentsAsyncTest()
        {
            //Arrange
            var components = BuildEntryComponents();

            SetupClient(components);

            var searchCriteria = new MedicationSearchCriteria
            {
                PatientId = Guid.NewGuid().ToString(),
                Status = "active",
            };

            var practitioners = SetupPractitionerBuilder(components);

            var medicationWithCompartments = SetupMedicationStatementWithCompartmentsBuilder(components, practitioners);

            IMedicationStatementWithCompartmentsAdapter adapter = new MedicationStatementWithCompartmentsAdapter(
                _mockedClient.Object,
                _r4mockedClient.Object,
                _mockedBuilder.Object,
                _r4mockedBuilder.Object,
                _mockPractitionerBuilder.Object);

            //Act
            var result = await adapter.FindMedicationsWithCompartmentsAsync(searchCriteria);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Should().Be(medicationWithCompartments[0]);
        }

        private static STU3.Bundle.EntryComponent[] BuildEntryComponents()
        {
            var components = new STU3.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new STU3.MedicationStatement
                    {
                        Id = Guid.NewGuid().ToString()
                    }
                }
            };
            return components;
        }

        private PractitionerWithRolesSTU3[] SetupPractitionerBuilder(STU3.Bundle.EntryComponent[] components)
        {
            var practitioners = new PractitionerWithRolesSTU3[]
            {
                new()
            };

            _mockPractitionerBuilder
                .Setup(builder => builder.RetrievePractitionerWithRolesAsync(components))
                .ReturnsAsync(practitioners);

            return practitioners;
        }

        private MedicationStatementWithCompartments[] SetupMedicationStatementWithCompartmentsBuilder(
            STU3.Bundle.EntryComponent[] components, PractitionerWithRolesSTU3[] practitionerWithRolesStu3s)
        {
            var medicationWithCompartments = new MedicationStatementWithCompartments[]
            {
                new()
                {
                    Resource = (STU3.MedicationStatement)components[0].Resource
                }
            };

            _mockedBuilder
                .Setup(builder => builder.ExtractMedicationWithCompartments(components, practitionerWithRolesStu3s))
                .Returns(medicationWithCompartments);
            return medicationWithCompartments;
        }

        private void SetupClient(STU3.Bundle.EntryComponent[] components)
        {
            _mockedClient
                .Setup(client => client.FindMedicationStatementsAsync(It.IsAny<SearchParams>()))
                .ReturnsAsync(components);
        }
    }
}

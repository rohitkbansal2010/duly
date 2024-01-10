// <copyright file="EncounterWithCompartmentsBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
extern alias stu3;

using Duly.Clinic.Fhir.Adapter.Builders.Implementations;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;
using STU3 = stu3::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Builders.Implementations
{
    [TestFixture]
    public class EncounterWithCompartmentsBuilderTests
    {
        private Mock<IPractitionerWithRolesBuilder> _mockedPractitionerWithRolesBuilder;
        private Mock<IFhirClientR4> _mockedClient;

        [SetUp]
        public void SetUp()
        {
            _mockedPractitionerWithRolesBuilder = new Mock<IPractitionerWithRolesBuilder>();
            _mockedClient = new Mock<IFhirClientR4>();
        }

        [Test]
        public async Task ExtractEncountersWithCompartmentsAsyncTest()
        {
            //Arrange
            var date = DateTime.Today.ToUniversalTime();
            var patientId = Guid.NewGuid().ToString();
            var practitionerId = Guid.NewGuid().ToString();
            var components = SetR4Components(patientId, practitionerId);

            SetupR4Client(patientId);

            SetupR4RetrievePractitionerWithRolesAsync(components, practitionerId);

            IEncounterWithCompartmentsBuilder builder = new EncounterWithCompartmentsBuilder(
                _mockedClient.Object,
                _mockedPractitionerWithRolesBuilder.Object);

            //Act
            var result = await builder.ExtractEncountersWithCompartmentsAsync(components, date);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Practitioners.Should().NotBeEmpty();
            result.First().Practitioners.Should().HaveCount(1);
            result.First().Practitioners.First().Resource.Id.Should().Be(practitionerId);
            result.First().Patient.Should().NotBeNull();
            result.First().Patient.Id.Should().Be(patientId);
        }

        [Test]
        public async Task ExtractEncountersWithCompartmentsAsync_EmptyResult_Test()
        {
            //Arrange
            IEncounterWithCompartmentsBuilder builder = new EncounterWithCompartmentsBuilder(
                _mockedClient.Object,
                _mockedPractitionerWithRolesBuilder.Object);

            //Act
            var result = await builder.ExtractEncountersWithCompartmentsAsync(Array.Empty<R4.Bundle.EntryComponent>(), default);

            //Assert
            result.Should().BeEmpty();
        }

        [Test]
        public async Task ExtractEncounterWithParticipantsAsyncTest()
        {
            //Arrange
            var relatedPersonId = Guid.NewGuid().ToString();
            var practitionerId = Guid.NewGuid().ToString();
            var components = SetR4Components(relatedPersonId, practitionerId);

            SetupR4RetrievePractitionerWithRolesAsync(components, practitionerId);

            IEncounterWithCompartmentsBuilder builder = new EncounterWithCompartmentsBuilder(
                _mockedClient.Object,
                _mockedPractitionerWithRolesBuilder.Object);

            //Act
            var result = await builder.ExtractEncounterWithParticipantsAsync(components);

            //Assert
            result.Should().NotBeNull();
            result.Practitioners.Should().NotBeEmpty();
            result.Practitioners.Should().HaveCount(1);
            result.Practitioners.First().Resource.Id.Should().Be(practitionerId);
        }

        [Test]
        public async Task ExtractEncountersWithAppointmentsAsync_Test()
        {
            //Arrange
            var relatedPersonId = Guid.NewGuid().ToString();
            var practitionerId = Guid.NewGuid().ToString();
            var components = SetSTU3Components(relatedPersonId, practitionerId);
            SetupSTU3RetrievePractitionerWithRolesAsync(components, practitionerId);

            IEncounterWithCompartmentsBuilder builder = new EncounterWithCompartmentsBuilder(
                null,
                _mockedPractitionerWithRolesBuilder.Object);

            //Act
            var result = await builder.ExtractEncountersWithAppointmentsAsync(components);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Practitioners.Should().NotBeEmpty();
            result.First().Practitioners.Should().HaveCount(1);
            result.First().Practitioners.First().Resource.Id.Should().Be(practitionerId);
        }

        [Test]
        public async Task ExtractEncountersWithAppointmentsAsync_EmptyArray_Test()
        {
            //Arrange
            IEncounterWithCompartmentsBuilder builder = new EncounterWithCompartmentsBuilder(
                null,
                _mockedPractitionerWithRolesBuilder.Object);

            //Act
            var result = await builder.ExtractEncountersWithAppointmentsAsync(Array.Empty<STU3.Bundle.EntryComponent>());

            //Assert
            result.Should().BeEmpty();
        }

        private static R4.Bundle.EntryComponent[] SetR4Components(string personId, string practitionerId)
        {
            return new R4.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new R4.Encounter
                    {
                        Id = Guid.NewGuid().ToString(),
                        Subject = new ResourceReference(nameof(R4.Patient) + "/" + personId),
                        Participant = new()
                        {
                            new R4.Encounter.ParticipantComponent
                            {
                                Individual = new ResourceReference(nameof(R4.Practitioner) + "/" + practitionerId)
                            }
                        }
                    }
                },
                new()
                {
                    Resource = new R4.Patient { Id = personId }
                }
            };
        }

        private static STU3.Bundle.EntryComponent[] SetSTU3Components(string personId, string practitionerId)
        {
            var appointmentId = Guid.NewGuid().ToString();
            return new STU3.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new STU3.Encounter
                    {
                        Id = Guid.NewGuid().ToString(),
                        Subject = new ResourceReference(nameof(STU3.Patient) + "/" + personId),
                        Participant = new()
                        {
                            new STU3.Encounter.ParticipantComponent
                            {
                                Individual = new ResourceReference(nameof(STU3.Practitioner) + "/" + practitionerId)
                            }
                        },
                        Appointment = new ResourceReference(appointmentId)
                    }
                },
                new()
                {
                    Resource = new STU3.Appointment { Id = appointmentId }
                },
            };
        }

        private void SetupR4RetrievePractitionerWithRolesAsync(R4.Bundle.EntryComponent[] components, string practitionerId)
        {
            var practitionersWithRoles = new PractitionerWithRoles[]
            {
                new()
                {
                    Resource = new() { Id = practitionerId },
                    Roles = new[]
                    {
                        new R4.PractitionerRole { Id = Guid.NewGuid().ToString() },
                    }
                }
            };

            _mockedPractitionerWithRolesBuilder
                .Setup(builder => builder.RetrievePractitionerWithRolesAsync(components, false))
                .Returns(Task.FromResult(practitionersWithRoles));
        }

        private void SetupSTU3RetrievePractitionerWithRolesAsync(STU3.Bundle.EntryComponent[] components, string practitionerId)
        {
            var practitionersWithRoles = new PractitionerWithRolesSTU3[]
            {
                new()
                {
                    Resource = new() { Id = practitionerId },
                    Roles = new[]
                    {
                        new STU3.PractitionerRole { Id = Guid.NewGuid().ToString() },
                    }
                }
            };

            _mockedPractitionerWithRolesBuilder
                .Setup(builder => builder.RetrievePractitionerWithRolesAsync(components))
                .Returns(Task.FromResult(practitionersWithRoles));
        }

        private void SetupR4Client(string patientId)
        {
            IEnumerable<R4.Encounter> pastEncounters = new R4.Encounter[]
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Subject = new ResourceReference(nameof(R4.Patient) + "/" + patientId)
                }
            };

            _mockedClient
                .Setup(client => client.FindResourcesAsync<R4.Encounter>(It.IsAny<SearchParams>()))
                .ReturnsAsync(pastEncounters);
        }
    }
}

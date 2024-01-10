// <copyright file="EncounterWithCompartmentsAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;
extern alias stu3;

using Duly.Clinic.Fhir.Adapter.Adapters.Implementations;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;
using STU3 = stu3::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class EncounterWithCompartmentsAdapterTests
    {
        private static readonly Dictionary<string, string> _inMemorySettings = new()
        {
            { "TmpEncountersDesiredDate", DateTime.Today.ToString(CultureInfo.InvariantCulture) }
        };

        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(_inMemorySettings)
            .Build();

        private Mock<IFhirClientR4> _mockedClientR4;
        private Mock<IFhirClientSTU3> _mockedClientStu3;
        private Mock<IEncounterWithCompartmentsBuilder> _mockedBuilder;

        [SetUp]
        public void SetUp()
        {
            _mockedClientR4 = new Mock<IFhirClientR4>();
            _mockedClientStu3 = new Mock<IFhirClientSTU3>();
            _mockedBuilder = new Mock<IEncounterWithCompartmentsBuilder>();
        }

        [Test]
        [Ignore("TBD in private API")]
        public async Task FindEncounterWithCompartmentsAsyncTest()
        {
            //Arrange
            var components = SetupR4FindEncountersAsync();

            var searchCriteria = new SearchCriteria
            {
                StartDateTime = DateTime.Today.ToUniversalTime()
            };

            var encounterWithCompartmentsArray = SetupExtractEncountersWithCompartmentsAsyncR4(
                components,
                searchCriteria);

            IEncounterWithCompartmentsAdapter adapter = new EncounterWithCompartmentsAdapter(_mockedClientR4.Object, null, _mockedBuilder.Object, _configuration);

            //Act
            var result = await adapter.FindEncountersWithCompartmentsAsync(searchCriteria);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Should().Be(encounterWithCompartmentsArray[0]);
        }

        [Test]
        public async Task FindEncounterWithParticipantsAsyncTest()
        {
            //Arrange
            var components = SetupR4FindEncountersAsync();

            var encounterId = "43";

            var encounterWithParticipants = SetupExtractEncountersWithCompartmentsByEncounterIdAsyncR4(components);

            IEncounterWithCompartmentsAdapter adapter = new EncounterWithCompartmentsAdapter(_mockedClientR4.Object, null, _mockedBuilder.Object, _configuration);

            //Act
            var result = await adapter.FindEncounterWithParticipantsAsync(encounterId);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(encounterWithParticipants);
        }

        [Test]
        public async Task FindEncountersWithAppointmentsAsyncTest()
        {
            //Arrange

            var searchCriteria = new EncounterSearchCriteria
            {
                PatientId = Guid.NewGuid().ToString()
            };

            var components = SetupSTU3FindEncountersAsync();

            var encounterWithCompartmentsArray = SetupExtractEncounterWithAppointmentAsyncSTU3(components);

            IEncounterWithCompartmentsAdapter adapter = new EncounterWithCompartmentsAdapter(null, _mockedClientStu3.Object, _mockedBuilder.Object, _configuration);

            //Act
            var result = await adapter.FindEncountersWithAppointmentsAsync(searchCriteria);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Should().Be(encounterWithCompartmentsArray[0]);
        }

        private EncounterWithCompartments[] SetupExtractEncountersWithCompartmentsAsyncR4(
            R4.Bundle.EntryComponent[] components,
            SearchCriteria searchCriteria)
        {
            var encounterWithCompartmentsArray = new EncounterWithCompartments[]
            {
                new ()
                {
                    Resource = (R4.Encounter)components[0].Resource,
                    Practitioners = new PractitionerWithRoles[]
                    {
                        new()
                        {
                            Resource = new R4.Practitioner()
                        }
                    }
                }
            };

            _mockedBuilder
                .Setup(builder => builder.ExtractEncountersWithCompartmentsAsync(components, searchCriteria.StartDateTime.Value))
                .Returns(Task.FromResult(encounterWithCompartmentsArray));

            return encounterWithCompartmentsArray;
        }

        private EncounterWithAppointment[] SetupExtractEncounterWithAppointmentAsyncSTU3(STU3.Bundle.EntryComponent[] components)
        {
            var encounterWithCompartmentsArray = new EncounterWithAppointment[]
            {
                new ()
                {
                    Resource = (STU3.Encounter)components[0].Resource,
                    Practitioners = new PractitionerWithRolesSTU3[]
                    {
                        new()
                        {
                            Resource = new STU3.Practitioner()
                        }
                    },
                    Appointment = new STU3.Appointment()
                }
            };

            _mockedBuilder
                .Setup(builder => builder.ExtractEncountersWithAppointmentsAsync(components))
                .Returns(Task.FromResult(encounterWithCompartmentsArray));

            return encounterWithCompartmentsArray;
        }

        private EncounterWithParticipants SetupExtractEncountersWithCompartmentsByEncounterIdAsyncR4(R4.Bundle.EntryComponent[] components)
        {
            var encounterWithParticipants = new EncounterWithParticipants
            {
                Resource = (R4.Encounter)components[0].Resource
            };

            _mockedBuilder
                .Setup(builder => builder.ExtractEncounterWithParticipantsAsync(components, true))
                .Returns(Task.FromResult(encounterWithParticipants));

            return encounterWithParticipants;
        }

        private R4.Bundle.EntryComponent[] SetupR4FindEncountersAsync()
        {
            R4.Bundle.EntryComponent[] components =
            {
                new ()
                {
                    Resource = new R4.Encounter
                    {
                        Period = new Period(FhirDateTime.Now(), FhirDateTime.Now())
                    }
                }
            };

            _mockedClientR4
                .Setup(client => client.FindEncountersAsync(It.IsAny<SearchParams>()))
                .Returns(Task.FromResult(components));

            return components;
        }

        private STU3.Bundle.EntryComponent[] SetupSTU3FindEncountersAsync()
        {
            STU3.Bundle.EntryComponent[] components =
            {
                new ()
                {
                    Resource = new STU3.Encounter
                    {
                        Period = new Period(FhirDateTime.Now(), FhirDateTime.Now())
                    }
                }
            };

            _mockedClientStu3
                .Setup(client => client.FindEncountersAsync(It.IsAny<SearchParams>()))
                .Returns(Task.FromResult(components));

            return components;
        }
    }
}

// <copyright file="FhirParticipantRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using AutoMapper;
using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using FluentAssertions;
using Hl7.Fhir.Model;
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
    public class FhirParticipantRepositoryTests
    {
        private Mock<IEncounterWithCompartmentsAdapter> _adapterMocked;
        private Mock<IMapper> _mapperMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IEncounterWithCompartmentsAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task GetParticipantsByEncounterIdAsyncTest()
        {
            //Arrange
            const string encounterId = "";
            var fhirEncounterWithParticipants = ConfigureFhirProvider();
            ConfigureMapper(fhirEncounterWithParticipants.Practitioners);
            IParticipantRepository repositoryMocked = new FhirParticipantRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var results = await repositoryMocked.GetParticipantsByEncounterIdAsync(encounterId);

            //Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(fhirEncounterWithParticipants.Practitioners.Length);
            results.First().Person.Id.Should().Be(fhirEncounterWithParticipants.Practitioners.First().Resource.Id);
        }

        private EncounterWithParticipants ConfigureFhirProvider()
        {
            EncounterWithParticipants encounterWithParticipants = new()
            {
                Practitioners = new PractitionerWithRoles[]
                {
                    new ()
                    {
                        Resource = new () { IdElement = new Id(Guid.NewGuid().ToString()) },
                        Roles = new[]
                        {
                            new R4.PractitionerRole { IdElement = new Id(Guid.NewGuid().ToString()) }
                        }
                    }
                }
            };

            _adapterMocked
                .Setup(provider => provider.FindEncounterWithParticipantsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(encounterWithParticipants));

            return encounterWithParticipants;
        }

        private void ConfigureMapper(PractitionerWithRoles[] practitioners)
        {
            var practitionersSystem = new List<PractitionerGeneralInfo>
            {
                new ()
                {
                    Id = practitioners.First().Resource.Id
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<PractitionerGeneralInfo>>(practitioners))
                .Returns(practitionersSystem);
        }
    }
}

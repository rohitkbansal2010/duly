// <copyright file="FhirEncounterRepositoryTests.cs" company="Duly Health and Care">
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
    public class FhirEncounterRepositoryTests
    {
        private Mock<IEncounterWithCompartmentsAdapter> _repositoryMocked;
        private Mock<IMapper> _mapperMocked;

        [SetUp]
        public void SetUp()
        {
            _repositoryMocked = new Mock<IEncounterWithCompartmentsAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task GetEncountersForSiteByDateAsyncTest()
        {
            //Arrange
            const string siteId = "";
            DateTime date = new();
            var fhirEncounters = ConfigureFhirProvider();
            ConfigureMapper(fhirEncounters);
            IEncounterRepository repositoryMocked = new FhirEncounterRepository(_repositoryMocked.Object, _mapperMocked.Object, null);

            //Act
            var results = await repositoryMocked.GetEncountersForSiteByDateAsync(siteId, date);

            //Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(fhirEncounters.Count());
            results.First().Id.Should().Be(fhirEncounters.First().Resource.Id);
        }

        private IEnumerable<EncounterWithCompartments> ConfigureFhirProvider()
        {
            IEnumerable<EncounterWithCompartments> encounters = new List<EncounterWithCompartments>
            {
                new ()
                {
                    Resource = new R4.Encounter
                    {
                        IdElement = new Hl7.Fhir.Model.Id(Guid.NewGuid().ToString())
                    }
                }
            };

            _repositoryMocked
                .Setup(provider => provider.FindEncountersWithCompartmentsAsync(It.IsAny<SearchCriteria>()))
                .Returns(Task.FromResult(encounters));

            return encounters;
        }

        private void ConfigureMapper(IEnumerable<EncounterWithCompartments> encounters)
        {
            var encountersSystem = new Encounter[]
            {
                new ()
                {
                    Id = encounters.First().Resource.Id
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<Encounter[]>(encounters))
                .Returns(encountersSystem);
        }
    }
}

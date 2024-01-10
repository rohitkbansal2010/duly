// <copyright file="FhirPractitionerGeneralInfoRepositoryTests.cs" company="Duly Health and Care">
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
using Hl7.Fhir.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class FhirPractitionerGeneralInfoRepositoryTests
    {
        private Mock<IPractitionerWithCompartmentsAdapter> _repositoryMocked;
        private Mock<IMapper> _mapperMocked;

        [SetUp]
        public void SetUp()
        {
            _repositoryMocked = new Mock<IPractitionerWithCompartmentsAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async System.Threading.Tasks.Task GetPractitionersBySiteIdAsyncTest()
        {
            //Arrange
            const string siteId = "";
            var fhirPractitioners = ConfigureFhirProvider();
            ConfigureMapper(fhirPractitioners);
            IPractitionerGeneralInfoRepository repositoryMocked = new FhirPractitionerGeneralInfoRepository(_repositoryMocked.Object, _mapperMocked.Object);

            //Act
            var results = await repositoryMocked.GetPractitionersBySiteIdAsync(siteId);

            //Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(fhirPractitioners.Count());
            results.First().Id.Should().Be(fhirPractitioners.First().Resource.Id);
        }

        private IEnumerable<PractitionerWithRoles> ConfigureFhirProvider()
        {
            IEnumerable<PractitionerWithRoles> practitionerRoles = new List<PractitionerWithRoles>
            {
                new ()
                {
                    Resource = new ()
                    {
                        IdElement = new Id(Guid.NewGuid().ToString())
                    },
                    Roles = new[]
                    {
                        new R4.PractitionerRole { IdElement = new Hl7.Fhir.Model.Id(Guid.NewGuid().ToString()) }
                    }
                }
            };

            _repositoryMocked
                .Setup(provider => provider.FindPractitionersWithRolesAsync(It.IsAny<SearchCriteria>()))
                .Returns(System.Threading.Tasks.Task.FromResult(practitionerRoles));

            return practitionerRoles;
        }

        private void ConfigureMapper(IEnumerable<PractitionerWithRoles> roles)
        {
            var systemPractitionerGeneralInfo = new List<PractitionerGeneralInfo>
            {
                new ()
                {
                    Id = roles.First().Resource.Id
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<PractitionerGeneralInfo>>(roles))
                .Returns(systemPractitionerGeneralInfo);
        }

        [Test]
        public async System.Threading.Tasks.Task GetPractitionersByIdentitiesAsyncTest()
        {
            //Arrange
            var identifiers = new[] { "EXTERNAL|123456" };
            var fhirPractitioners = ConfigureFhirProviderForIdentities();
            ConfigureMapper(fhirPractitioners);
            IPractitionerGeneralInfoRepository repositoryMocked = new FhirPractitionerGeneralInfoRepository(_repositoryMocked.Object, _mapperMocked.Object);

            //Act
            var results = await repositoryMocked.GetPractitionersByIdentifiersAsync(identifiers);

            //Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(fhirPractitioners.Count());
            results.First().Id.Should().Be(fhirPractitioners.First().Resource.Id);
        }

        private IEnumerable<PractitionerWithRoles> ConfigureFhirProviderForIdentities()
        {
            IEnumerable<PractitionerWithRoles> practitionerRoles = new List<PractitionerWithRoles>
            {
                new ()
                {
                    Resource = new ()
                    {
                        IdElement = new Id(Guid.NewGuid().ToString())
                    },
                    Roles = new[]
                    {
                        new R4.PractitionerRole { IdElement = new Hl7.Fhir.Model.Id(Guid.NewGuid().ToString()) }
                    }
                }
            };

            _repositoryMocked
                .Setup(provider => provider.FindPractitionersByIdentifiersAsync(It.IsAny<string[]>()))
                .Returns(System.Threading.Tasks.Task.FromResult(practitionerRoles));

            return practitionerRoles;
        }
    }
}

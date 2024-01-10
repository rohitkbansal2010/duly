// <copyright file="FhirPatientRepositoryTests.cs" company="Duly Health and Care">
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
    public class FhirPatientRepositoryTests
    {
        private Mock<IPatientWithCompartmentsAdapter> _adapterMocked;
        private Mock<IMapper> _mapperMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IPatientWithCompartmentsAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task GetPatientGeneralInfoByIdAsyncTest()
        {
            //Arrange
            var patientId = Guid.NewGuid().ToString();
            var fhirPatient = ConfigureFhirProvider(patientId);
            ConfigureMapper(fhirPatient);
            IPatientRepository repositoryMocked = new FhirPatientRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var results = await repositoryMocked.GetPatientByIdAsync(patientId);

            //Assert
            results.Should().NotBeNull();
            results.PatientGeneralInfo.Id.Should().Be(fhirPatient.Resource.Id);
        }

        private PatientWithCompartments ConfigureFhirProvider(string patientId)
        {
            PatientWithCompartments patient = new()
            {
                Resource = new R4.Patient
                {
                    IdElement = new Hl7.Fhir.Model.Id(patientId)
                }
            };

            _adapterMocked
                .Setup(provider => provider.FindPatientByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(patient));

            return patient;
        }

        private void ConfigureMapper(PatientWithCompartments patient)
        {
            Patient patientSystem = new()
            {
                PatientGeneralInfo = new()
                {
                    Id = patient.Resource.Id
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<Patient>(patient))
                .Returns(patientSystem);
        }

        [Test]
        public async Task GetPatientGeneralInfoByIdentitiesAsyncTest()
        {
            //Arrange
            var identifiers = new[] { "EXTERNAL|123456" };
            var fhirPatients = ConfigureFhirProviderForIdentities();
            ConfigureMapper(fhirPatients);
            IPatientRepository repositoryMocked = new FhirPatientRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var results = await repositoryMocked.GetPatientsByIdentifiersAsync(identifiers);

            //Assert
            results.Should().NotBeNull();
            results.First().PatientGeneralInfo.Id.Should().Be(fhirPatients.First().Resource.Id);
        }

        private void ConfigureMapper(IEnumerable<PatientWithCompartments> patients)
        {
            IEnumerable<Patient> patientsSystem = new[]
            {
                new Patient()
                {
                    PatientGeneralInfo = new()
                    {
                        Id = patients.First().Resource.Id
                    }
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Patient>>(patients))
                .Returns(patientsSystem);
        }

        private IEnumerable<PatientWithCompartments> ConfigureFhirProviderForIdentities()
        {
            IEnumerable<PatientWithCompartments> patients = new[]
            {
                new PatientWithCompartments()
                {
                    Resource = new R4.Patient
                    {
                        IdElement = new Hl7.Fhir.Model.Id(Guid.NewGuid().ToString())
                    }
                }
            };

            _adapterMocked
                .Setup(provider => provider.FindPatientsByIdentifiersAsync(It.IsAny<string[]>()))
                .Returns(Task.FromResult(patients));

            return patients;
        }
    }
}

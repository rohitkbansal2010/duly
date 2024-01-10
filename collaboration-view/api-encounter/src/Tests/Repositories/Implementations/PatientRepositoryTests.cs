// <copyright file="PatientRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class PatientRepositoryTests
    {
        private Mock<IEncounterContext> _encounterContextMock;
        private Mock<IClient> _clientMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IPatientsClient> _patientsClient;

        [SetUp]
        public void SetUp()
        {
            ConfigureEncounterContext();
            _clientMock = new Mock<IClient>();
            _mapperMock = new Mock<IMapper>();
            _patientsClient = new Mock<IPatientsClient>();
        }

        [Test]
        public async Task GetPatientByIdAsyncTest()
        {
            //Arrange
            const string patientId = "123";
            var epicPatient = BuildEpicPatient(patientId);
            ConfigureMapper(epicPatient);
            IPatientRepository repositoryMock = new PatientRepository(
                _encounterContextMock.Object,
                _mapperMock.Object,
                _clientMock.Object,
                _patientsClient.Object);

            //Act
            var results = await repositoryMock.GetPatientByIdAsync(patientId);

            //Assert
            _clientMock.Verify(x => x.PatientsGetAsync(patientId, It.IsAny<Guid>(), default), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<Models.Patient>();
            results.PatientGeneralInfo.Should().NotBeNull();
            results.PatientGeneralInfo.Id.Should().Be(epicPatient.PatientGeneralInfo.Id);
            results.BirthDate.Should().Be(new DateTimeOffset(new DateTime(1970, 7, 15)));
            results.Gender.Should().Be(Models.Gender.Female);
        }

        [Test]
        public async Task GetPatientsByIdsAsyncTest()
        {
            //Arrange
            string[] patientIds = { "123" };
            var epicPatients = BuildEpicPatients(patientIds);
            ConfigureMapper(epicPatients);

            IPatientRepository repositoryMock = new PatientRepository(
                _encounterContextMock.Object,
                _mapperMock.Object,
                _clientMock.Object,
                _patientsClient.Object);

            //Act
            var results = await repositoryMock.GetPatientsByIdsAsync(patientIds);

            //Assert
            _clientMock.Verify(x => x.PatientsGetAsync(patientIds, It.IsAny<Guid>(), default), Times.Once());

            results.Should().NotBeNull();
            results.Should().HaveCount(1);

            var firstPatient = results.First();

            firstPatient.PatientGeneralInfo.Should().NotBeNull();
            firstPatient.PatientGeneralInfo.Id.Should().Be(patientIds[0]);
            firstPatient.BirthDate.Should().Be(new DateTimeOffset(new DateTime(1970, 7, 15)));
            firstPatient.Gender.Should().Be(Models.Gender.Female);
        }

        [Test]
        public async Task GetPatientsByIdsAsync_ReturnEmpty_Test()
        {
            //Arrange
            var patientIds = Array.Empty<string>();

            IPatientRepository repositoryMock = new PatientRepository(
                _encounterContextMock.Object,
                _mapperMock.Object,
                _clientMock.Object,
                _patientsClient.Object);

            //Act
            var results = await repositoryMock.GetPatientsByIdsAsync(patientIds);

            //Assert
            _clientMock.VerifyNoOtherCalls();
            results.Should().BeEmpty();
        }

        private void ConfigureEncounterContext()
        {
            _encounterContextMock = new Mock<IEncounterContext>();

            _encounterContextMock
                .Setup(ctx => ctx.GetXCorrelationId())
                .Returns(Guid.NewGuid());
        }

        private Patient BuildEpicPatient(string patientId)
        {
            var epicPatient = new Patient
            {
                PatientGeneralInfo = new PatientGeneralInfo
                {
                    Id = Guid.NewGuid().ToString()
                },
                BirthDate = new DateTimeOffset(new DateTime(1970, 7, 15)),
                Gender = Gender.Female,
            };

            _clientMock
                .Setup(client => client.PatientsGetAsync(patientId, It.IsAny<Guid>(), default))
                .Returns(Task.FromResult(epicPatient));

            return epicPatient;
        }

        private ICollection<Patient> BuildEpicPatients(string[] patientIds)
        {
            ICollection<Patient> epicPatients = patientIds
                .Select(pId =>
                    new Patient
                    {
                        PatientGeneralInfo = new PatientGeneralInfo
                        {
                            Id = pId
                        },
                        BirthDate = new DateTimeOffset(new DateTime(1970, 7, 15)),
                        Gender = Gender.Female,
                    })
                .ToArray();

            _clientMock
                .Setup(client => client.PatientsGetAsync(patientIds, It.IsAny<Guid>(), default))
                .Returns(Task.FromResult(epicPatients));

            return epicPatients;
        }

        private void ConfigureMapper(Patient epicPatient)
        {
            var patient = new Models.Patient
            {
                PatientGeneralInfo = new Models.PatientGeneralInfo
                {
                    Id = epicPatient.PatientGeneralInfo.Id
                },
                BirthDate = epicPatient.BirthDate,
                Gender = Models.Gender.Female,
            };

            _mapperMock
                .Setup(mapper => mapper.Map<Models.Patient>(epicPatient))
                .Returns(patient);
        }

        private void ConfigureMapper(ICollection<Patient> epicPatients)
        {
            IEnumerable<Models.Patient> patients = epicPatients.Select(p =>
                new Models.Patient
                {
                    PatientGeneralInfo = new Models.PatientGeneralInfo
                    {
                        Id = p.PatientGeneralInfo.Id
                    },
                    BirthDate = p.BirthDate,
                    Gender = Models.Gender.Female,
                });

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<Models.Patient>>(epicPatients))
                .Returns(patients);
        }
    }
}

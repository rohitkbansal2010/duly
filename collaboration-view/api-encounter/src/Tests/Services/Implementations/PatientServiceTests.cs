// <copyright file="PatientServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class PatientServiceTests
    {
        private Mock<IPatientRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetPatientByIdAsyncTest()
        {
            //Arrange
            const string patientId = "123";
            var patient = BuildPatient(patientId);
            ConfigureMapper(patient);
            var serviceMock = new PatientService(
                _mapperMock.Object,
                _repositoryMock.Object);

            //Act
            var results = await serviceMock.GetPatientByIdAsync(patientId);

            //Assert
            _repositoryMock.Verify(x => x.GetPatientByIdAsync(patientId), Times.Once());

            results.Should().NotBeNull();
            results.Should().BeOfType<ApiContracts.Patient>();
            results.GeneralInfo.Should().NotBeNull();
            results.GeneralInfo.Id.Should().Be(patient.PatientGeneralInfo.Id);
            results.BirthDate.Should().Be(new DateTime(1970, 7, 15));
            results.Gender.Should().Be(ApiContracts.Gender.Female);
        }

        private Patient BuildPatient(string patientId)
        {
            var patient = new Patient
            {
                PatientGeneralInfo = new PatientGeneralInfo
                {
                    Id = Guid.NewGuid().ToString()
                },
                BirthDate = new DateTimeOffset(new DateTime(1970, 7, 15)),
                Gender = Gender.Female,
                Identifiers = new string[] { "EXTERNAL|777777" },
            };

            _repositoryMock
                .Setup(repo => repo.GetPatientByIdAsync(patientId))
                .Returns(Task.FromResult(patient));

            var param = new PatientPhotoByParam
            {
                PatientID = "777777",
                PatientIDType = "EXTERNAL"
            };

            var listOfPatientPhoto = new List<PatientPhoto>()
            {
                new PatientPhoto
                {
                     FileExtension = ".jpg",
                     FileSize = 11987,
                     Photo = "Test",
                     Title = "Photo"
                }
            };

            _repositoryMock
               .Setup(repo => repo.GetPatientsPhotoByIdsAsync(param))
               .Returns(Task.FromResult(listOfPatientPhoto));

            return patient;
        }

        private List<PatientPhoto> BuildPatientPhoto(PatientPhotoByParam patientPhotoByParam)
        {
            var param = new PatientPhotoByParam
            {
                PatientID = "777777",
                PatientIDType = "EXTERNAL"
            };

            var listOfPatientPhoto = new List<PatientPhoto>()
            {
                new PatientPhoto
                {
                     FileExtension = ".jpg",
                     FileSize = 11987,
                     Photo = "Test",
                     Title = "Photo"
                }
            };

            _repositoryMock
               .Setup(repo => repo.GetPatientsPhotoByIdsAsync(param))
               .Returns(Task.FromResult(listOfPatientPhoto));

            return listOfPatientPhoto;
        }

        private void ConfigureMapper(Patient patient)
        {
            var apiPatient = new ApiContracts.Patient
            {
                GeneralInfo = new ApiContracts.PatientGeneralInfo
                {
                    Id = patient.PatientGeneralInfo.Id
                },
                BirthDate = patient.BirthDate.Date,
                Gender = ApiContracts.Gender.Female,
            };

            _mapperMock
                .Setup(mapper => mapper.Map<ApiContracts.Patient>(patient))
                .Returns(apiPatient);
        }
    }
}
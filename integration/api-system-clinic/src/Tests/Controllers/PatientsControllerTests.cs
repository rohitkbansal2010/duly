// <copyright file="PatientsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Controllers;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Common.Infrastructure.Exceptions;
using Duly.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Tests.Controllers
{
    [TestFixture]
    public class PatientsControllerTests
    {
        private Mock<IPatientRepository> _patientRepositoryMocked;
        private Mock<ILogger<PatientsController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _patientRepositoryMocked = new Mock<IPatientRepository>();
            _loggerMocked = new Mock<ILogger<PatientsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetPatientById_Test()
        {
            //Arrange
            var patientId = Guid.NewGuid().ToString();
            var patient = ConfigureRepository(patientId);
            var patientsController = new PatientsController(_patientRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);

            patientsController.MockObjectValidator();

            //Act
            var actionResult = await patientsController.GetPatientById(patientId);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (Patient)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNull();
            responseData.PatientGeneralInfo.Id.Should().Be(patient.PatientGeneralInfo.Id);
        }

        [Test]
        public async Task GetPatientById_EntityNotFoundException_Test()
        {
            //Arrange
            var patientId = "test-patient-id";
            var patientsController = new PatientsController(_patientRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);

            patientsController.MockObjectValidator();

            //Act
            Func<Task> action = async () => await patientsController.GetPatientById(patientId);

            //Assert
            var result = await action.Should().ThrowAsync<EntityNotFoundException>();
            result.Which.Message.Should().Be("Patient with ID test-patient-id was not found.");
        }

        private Patient ConfigureRepository(string patientId)
        {
            Patient patient = new()
            {
                PatientGeneralInfo = new()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _patientRepositoryMocked
                .Setup(repository => repository.GetPatientByIdAsync(patientId))
                .ReturnsAsync(patient);

            return patient;
        }

        [Test]
        public async Task GetPatientByIdentities_Test()
        {
            //Arrange
            var patientIdentities = new[] { "EXTERNAL|123456" };
            var patients = ConfigureRepositoryForIdentities(patientIdentities);
            var patientsController = new PatientsController(_patientRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);

            patientsController.MockObjectValidator();

            //Act
            var actionResult = await patientsController.GetPatientsByIdentifiers(patientIdentities);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<Patient>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNull();
            responseData.First().PatientGeneralInfo.Id.Should().Be(patients.First().PatientGeneralInfo.Id);
        }

        private IEnumerable<Patient> ConfigureRepositoryForIdentities(string[] patientExternalIdentities)
        {
            Patient[] patients = new[]
            {
                new Patient
                {
                    PatientGeneralInfo = new()
                    {
                        Id = Guid.NewGuid().ToString()
                    }
                }
            };

            _patientRepositoryMocked
                .Setup(repository => repository.GetPatientsByIdentifiersAsync(patientExternalIdentities))
                .ReturnsAsync(patients);

            return patients;
        }
    }
}

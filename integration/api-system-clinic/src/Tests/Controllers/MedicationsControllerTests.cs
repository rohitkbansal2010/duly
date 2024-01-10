// <copyright file="MedicationsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Api.Controllers;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
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
    public class MedicationsControllerTests
    {
        private Mock<IMedicationRepository> _medicationRepositoryMocked;
        private Mock<ILogger<MedicationsController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _medicationRepositoryMocked = new Mock<IMedicationRepository>();
            _loggerMocked = new Mock<ILogger<MedicationsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [TestCase(MedicationStatus.Inactive, null)]
        [TestCase(MedicationStatus.Active, null)]
        [TestCase(null, null)]
        [TestCase(null, new MedicationCategory[0])]
        [TestCase(null, new[] { MedicationCategory.Community })]
        [TestCase(null, new[] { MedicationCategory.Community, MedicationCategory.PatientSpecified })]
        public async Task GetMedicationsByPatientIdTest(MedicationStatus? medicationStatus, MedicationCategory[] medicationCategories)
        {
            //Arrange
            const string patientId = "test";
            var medications = ConfigureRepository(patientId, medicationStatus, medicationCategories?.Any() ?? false ? medicationCategories : null);
            var controller = new MedicationsController(_medicationRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.GetMedicationsByPatientId(patientId, medicationStatus, medicationCategories);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<Medication>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNullOrEmpty();
            responseData.Should().HaveCount(medications.Count());
            var firstItem = responseData.First();
            firstItem.Id.Should().Be(medications.First().Id);
            firstItem.Status.Should().Be(medications.First().Status);
        }

        private IEnumerable<Medication> ConfigureRepository(string patientId, MedicationStatus? medicationStatus, MedicationCategory[] medicationCategories)
        {
            IEnumerable<Medication> medications = new List<Medication>
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = medicationStatus.GetValueOrDefault()
                }
            };

            _medicationRepositoryMocked
                .Setup(repository => repository.FindMedicationsForPatientAsync(patientId, medicationStatus, medicationCategories))
                .Returns(Task.FromResult(medications));

            return medications;
        }
    }
}
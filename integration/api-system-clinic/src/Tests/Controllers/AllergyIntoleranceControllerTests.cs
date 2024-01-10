// <copyright file="AllergyIntoleranceControllerTests.cs" company="Duly Health and Care">
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
    public class AllergyIntoleranceControllerTests
    {
        private Mock<IAllergyIntoleranceRepository> _allergyIntoleranceRepositoryMocked;
        private Mock<ILogger<AllergyIntoleranceController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _allergyIntoleranceRepositoryMocked = new Mock<IAllergyIntoleranceRepository>();
            _loggerMocked = new Mock<ILogger<AllergyIntoleranceController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [TestCase(ClinicalStatus.Active)]
        public async Task GetConfirmedAllergyIntoleranceForSpecificPatientTest(ClinicalStatus clinicalStatus)
        {
            //Arrange
            const string patientId = "test";
            var allergyIntolerance = ConfigureRepository(patientId, clinicalStatus);
            var controller = new AllergyIntoleranceController(_allergyIntoleranceRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.GetConfirmedAllergyIntoleranceForSpecificPatient(patientId, clinicalStatus);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<AllergyIntolerance>)((OkObjectResult)actionResult.Result).Value;

            if (clinicalStatus == ClinicalStatus.Active)
            {
                responseData.Should().NotBeNullOrEmpty();
                responseData.Should().HaveCount(allergyIntolerance.Count());
                var firstItem = responseData.First();
                firstItem.Id.Should().Be(allergyIntolerance.First().Id);
            }
        }

        private IEnumerable<AllergyIntolerance> ConfigureRepository(string patientId, ClinicalStatus clinicalStatus)
        {
            var allergyIntolerance = new List<AllergyIntolerance>();

            if (clinicalStatus == ClinicalStatus.Active)
            {
                allergyIntolerance.Add(new AllergyIntolerance
                {
                    Id = Guid.NewGuid().ToString()
                });
                allergyIntolerance.Add(new AllergyIntolerance
                {
                    Id = Guid.NewGuid().ToString()
                });
                allergyIntolerance.Add(new AllergyIntolerance
                {
                    Id = Guid.NewGuid().ToString()
                });
            }

            _allergyIntoleranceRepositoryMocked
                .Setup(repository => repository.GetConfirmedAllergyIntoleranceForPatientAsync(patientId, clinicalStatus))
                .Returns(Task.FromResult(allergyIntolerance.AsEnumerable()));

            return allergyIntolerance;
        }
    }
}
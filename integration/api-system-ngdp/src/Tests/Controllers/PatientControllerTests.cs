// <copyright file="PatientControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Testing;
using Duly.Ngdp.Api.Controllers;
using Duly.Ngdp.Api.Repositories.Interfaces;
using Duly.Ngdp.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Tests.Controllers
{
    [TestFixture]
    public class PatientControllerTests
    {
        private Mock<IPatientRepository> _patientRepositoryMocked;
        private Mock<ILogger<PatientController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _patientRepositoryMocked = new Mock<IPatientRepository>();
            _loggerMocked = new Mock<ILogger<PatientController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetPatientByReferralId_Test()
        {
            //Arrange
            const string referralId = "1";

            var patient = SetupRepository(referralId);

            var controller = new PatientController(_patientRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.GetPatientByReferralId(referralId);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (ReferralPatient)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(patient);
        }

        private ReferralPatient SetupRepository(string referralId)
        {
            var patient = new ReferralPatient();

            _patientRepositoryMocked
                .Setup(repository =>
                    repository.GetPatientByReferralIdAsync(referralId))
                .ReturnsAsync(patient);

            return patient;
        }
    }
}

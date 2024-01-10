// <copyright file="ImmunizationsControllerTests.cs" company="Duly Health and Care">
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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.Ngdp.Api.Tests.Controllers
{
    [TestFixture]
    public class ImmunizationsControllerTests
    {
        private Mock<IImmunizationRepository> _repositoryMocked;
        private Mock<ILogger<ImmunizationsController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _repositoryMocked = new Mock<IImmunizationRepository>();
            _loggerMocked = new Mock<ILogger<ImmunizationsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetImmunizationsForSpecificPatientTest()
        {
            //Arrange
            const string patientId = "309";
            var includedDueStatuses = new[] { DueStatus.DueOn, DueStatus.DueSoon };

            var controller = new ImmunizationsController(_loggerMocked.Object, _iWebHostEnvironment.Object, _repositoryMocked.Object);
            controller.MockObjectValidator();

            var immunizations = SetupRepository(patientId, includedDueStatuses);

            //Act
            var actionResult = await controller.GetImmunizationsForSpecificPatient(patientId, includedDueStatuses);

            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<Immunization>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().BeEquivalentTo(immunizations);
        }

        private IEnumerable<Immunization> SetupRepository(string patientId, DueStatus[] includedDueStatuses)
        {
            IEnumerable<Immunization> immunizations = new Immunization[]
            {
                new()
            };

            _repositoryMocked
                .Setup(repository =>
                    repository.GetImmunizationsForSpecificPatientAsync(patientId, includedDueStatuses))
                .ReturnsAsync(immunizations);

            return immunizations;
        }
    }
}
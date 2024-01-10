// <copyright file="ImmunizationsControllerTests.cs" company="Duly Health and Care">
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
    public class ImmunizationsControllerTests
    {
        private Mock<IImmunizationRepository> _immunizationRepositoryMocked;
        private Mock<ILogger<ImmunizationsController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _immunizationRepositoryMocked = new Mock<IImmunizationRepository>();
            _loggerMocked = new Mock<ILogger<ImmunizationsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [TestCase(null)]
        [TestCase(new ImmunizationStatus[0])]
        [TestCase(new[] { ImmunizationStatus.Completed })]
        [TestCase(new[] { ImmunizationStatus.Completed, ImmunizationStatus.NotDone })]
        public async Task GetImmunizationsByPatientIdAndStatusesTest(ImmunizationStatus[] immunizationStatuses)
        {
            //Arrange
            const string patientId = "test";
            var immunizations = ConfigureRepository(patientId, immunizationStatuses?.Any() ?? false ? immunizationStatuses : null);
            var controller = new ImmunizationsController(_loggerMocked.Object, _iWebHostEnvironment.Object, _immunizationRepositoryMocked.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.FindImmunizationsForPatient(patientId, immunizationStatuses);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<Immunization>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNullOrEmpty();
            responseData.Should().HaveCount(immunizations.Count());
            var firstItem = responseData.First();
            firstItem.Id.Should().Be(immunizations.First().Id);
            firstItem.Status.Should().Be(immunizations.First().Status);
        }

        private IEnumerable<Immunization> ConfigureRepository(string patientId, ImmunizationStatus[] immunizationStatuses)
        {
            IEnumerable<Immunization> immunizations = new Immunization[]
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = immunizationStatuses?.FirstOrDefault() ?? default
                }
            };

            _immunizationRepositoryMocked
                .Setup(repository => repository.FindImmunizationsForPatientAsync(patientId, immunizationStatuses))
                .Returns(Task.FromResult(immunizations));

            return immunizations;
        }
    }
}
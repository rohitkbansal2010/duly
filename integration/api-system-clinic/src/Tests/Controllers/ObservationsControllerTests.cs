// <copyright file="ObservationsControllerTests.cs" company="Duly Health and Care">
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
    public class ObservationsControllerTests
    {
        private Mock<IObservationRepository> _observationRepositoryMocked;
        private Mock<ILogger<ObservationsController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _observationRepositoryMocked = new Mock<IObservationRepository>();
            _loggerMocked = new Mock<ILogger<ObservationsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetObservationsByPatientIdTest()
        {
            //Arrange
            const string patientId = "test";
            var observationTypes = new[]
            {
                ObservationType.BloodPressure,
            };

            var lowerBoundDate = DateTime.Now;
            var upperBoundBoundDate = DateTime.Now;

            var observations = ConfigureRepository(patientId, observationTypes, lowerBoundDate, upperBoundBoundDate);

            var controller = new ObservationsController(_observationRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.GetObservationsByPatientId(patientId, observationTypes, lowerBoundDate, upperBoundBoundDate);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<Observation>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNullOrEmpty();
            responseData.Should().HaveCount(observations.Count());
            var firstItem = responseData.First();
            firstItem.Type.Should().Be(observations.First().Type);
            firstItem.Components.First().Type.Should().Be(observations.First().Components.First().Type);
        }

        [Test]
        public async Task GetLastObservationsByPatientIdTest()
        {
            //Arrange
            const string patientId = "test";
            var observationTypes = new[]
            {
                ObservationType.BloodPressure,
            };
            var observations = ConfigureRepository(patientId, observationTypes, DateTime.Now, DateTime.Now);

            var controller = new ObservationsController(_observationRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.GetLastObservationsByPatientId(patientId, observationTypes);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<Observation>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNullOrEmpty();
            responseData.Should().HaveCount(observations.Count());
            var firstItem = responseData.First();
            firstItem.Type.Should().Be(observations.First().Type);
            firstItem.Components.First().Type.Should().Be(observations.First().Components.First().Type);
        }

        private IEnumerable<Observation> ConfigureRepository(string patientId, ObservationType[] observationTypes, DateTime lowerBoundDate, DateTime upperBoundBoundDate)
        {
            IEnumerable<Observation> observations = new List<Observation>
            {
                new ()
                {
                    Type = ObservationType.BloodPressure,
                    Components = new ObservationComponent[]
                    {
                        new()
                        {
                            Type = ObservationComponentType.Diastolic,
                        }
                    },
                }
            };

            _observationRepositoryMocked
                .Setup(repository => repository.FindObservationsForPatientAsync(patientId, observationTypes, lowerBoundDate, upperBoundBoundDate))
                .Returns(Task.FromResult(observations));
            _observationRepositoryMocked
                .Setup(repository => repository.FindLastObservationsForPatientAsync(patientId, observationTypes))
                .Returns(Task.FromResult(observations));

            return observations;
        }
    }
}

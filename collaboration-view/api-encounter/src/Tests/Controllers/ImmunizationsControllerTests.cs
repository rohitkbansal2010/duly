// <copyright file="ImmunizationsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Controllers;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    [SetCulture("en-us")]
    public class ImmunizationsControllerTests
    {
        public const string ADMINISTEREDTITLE = "ADMINISTERED";
        public const string NOTADMINISTEREDTITLE = "NOT ADMINISTERED";

        private Mock<ILogger<ImmunizationsController>> _loggerMock;
        private Mock<IImmunizationService> _serviceMock;

        private ImmunizationsController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<ImmunizationsController>>();
            _serviceMock = new Mock<IImmunizationService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new ImmunizationsController(_serviceMock.Object, _loggerMock.Object, iWebHostEnvironment.Object);
        }

        [Test]
        public async Task GetImmunizationByPatientIdTest()
        {
            //Arrange
            const string patientId = "test-patient-id";

            var serviceResult = SetupService(patientId);

            _controller.MockObjectValidator();

            //Act
            var result = await _controller.GetImmunizationByPatientId(patientId);

            //Assert
            _serviceMock.Verify(
                x => x.GetImmunizationsByPatientIdAsync(patientId),
                Times.Once());

            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as Immunizations;

            contentResult.Should().NotBeNull();

            contentResult.Progress.Should().NotBeNull();
            contentResult.Progress.PercentageCompletion.Should().Be(serviceResult.Progress.PercentageCompletion);

            contentResult.RecommendedImmunizations.Should().NotBeNullOrEmpty();
            contentResult.RecommendedImmunizations.Should().HaveCount(serviceResult.RecommendedImmunizations.Length);

            contentResult.PastImmunizations.Should().NotBeNullOrEmpty();
            contentResult.PastImmunizations.Should().HaveCount(serviceResult.PastImmunizations.Length);
        }

        private Immunizations SetupService(string patientId)
        {
            var immunizations = new Immunizations
            {
                RecommendedImmunizations = new[]
                {
                    new ImmunizationsRecommendedGroup
                    {
                        Title = "TDAP",
                        Vaccinations = new[]
                        {
                            new RecommendedVaccination
                            {
                                Title = "Tdap",
                                Status = RecommendedVaccinationStatus.Completed,
                                DateTitle = ADMINISTEREDTITLE,
                                Date = new DateTimeOffset(new DateTime(2002, 3, 11))
                            },
                        }
                    },
                    new ImmunizationsRecommendedGroup
                    {
                        Title = "FLU",
                        Vaccinations = new[]
                        {
                            new RecommendedVaccination
                            {
                                Title = "Influenza, seasonal, injectable",
                                Status = RecommendedVaccinationStatus.Addressed,
                                DateTitle = ADMINISTEREDTITLE,
                                Date = new DateTimeOffset(new DateTime(2017, 9, 3))
                            },
                        }
                    }
                },
                PastImmunizations = new[]
                {
                    new ImmunizationsGroup
                    {
                        Title = "HepB",
                        Vaccinations = new[]
                        {
                            new Vaccination
                            {
                                Title = "Hep B-dialysis",
                                DateTitle = ADMINISTEREDTITLE,
                                Date = new DateTimeOffset(new DateTime(1998, 2, 15)),
                                Dose = new Dose
                                {
                                    Amount = 1.0M,
                                    Unit = "mL"
                                }
                            }
                        }
                    },
                    new ImmunizationsGroup
                    {
                        Title = "MENING",
                        Vaccinations = new[]
                        {
                            new Vaccination
                            {
                                Title = "Meningococcal MCV4O",
                                DateTitle = NOTADMINISTEREDTITLE
                            }
                        }
                    }
                }
            };

            _serviceMock
                .Setup(x => x.GetImmunizationsByPatientIdAsync(patientId))
                .Returns(Task.FromResult(immunizations));

            return immunizations;
        }
    }
}
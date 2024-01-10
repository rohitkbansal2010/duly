// <copyright file="VitalsControllerTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Controllers;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
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

namespace Duly.CollaborationView.Encounter.Api.Tests.Controllers
{
    [TestFixture]
    public class VitalsControllerTests
    {
        private const string PatientId = "test";
        private const VitalsCardType VitalsCardTypeActual = VitalsCardType.WeightAndHeight;

        private VitalsController _controller;

        [SetUp]
        public void SetUp()
        {
            var loggerMock = new Mock<ILogger<VitalsController>>();
            var vitalServiceMocked = new Mock<IVitalService>();

            vitalServiceMocked.Setup(x => x.GetLatestVitalsForPatientAsync(PatientId))
                .ReturnsAsync(new List<VitalsCard> { new() { CardType = VitalsCardType.BloodOxygen } });

            vitalServiceMocked.Setup(x => x.GetVitalHistoryForPatientByVitalsCardType(PatientId, It.IsAny<DateTime>(), VitalsCardTypeActual))
                .ReturnsAsync(new VitalHistory());
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            _controller = new VitalsController(vitalServiceMocked.Object, loggerMock.Object, iWebHostEnvironment.Object);

            _controller.MockObjectValidator();
        }

        [Test]
        public async Task GetVitalsCardsTest()
        {
            //Arrange

            //Act
            var result = await _controller.GetVitalsCards(PatientId);

            //Assert
            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as IEnumerable<VitalsCard>;
            contentResult.Should().NotBeNullOrEmpty();
            contentResult.First().CardType.Should().Be(VitalsCardType.BloodOxygen);
        }

        [Test]
        public async Task GetChartByVitalsCardTypeTest()
        {
            //Arrange

            //Act
            var result = await _controller.GetChartByVitalsCardType(PatientId, VitalsCardTypeActual);

            //Assert
            var okResult = result.Result as OkObjectResult;
            var contentResult = okResult.Value as VitalHistory;
            contentResult.Should().NotBeNull();
        }

        [Test]
        public async Task GetChartByVitalsCardType_EntityNotFoundException_Test()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<VitalsController>>();
            var vitalServiceMocked = new Mock<IVitalService>();
            var iWebHostEnvironment = new Mock<IWebHostEnvironment>();

            var controller = new VitalsController(vitalServiceMocked.Object, loggerMock.Object, iWebHostEnvironment.Object);

            controller.MockObjectValidator();

            //Act
            Func<Task> action = async () => await controller.GetChartByVitalsCardType(PatientId, VitalsCardTypeActual);

            //Assert
            var result = await action.Should().ThrowAsync<EntityNotFoundException>();
            result.Which.Message.Should().Be("vitalHistory with ID test was not found.");
        }
    }
}

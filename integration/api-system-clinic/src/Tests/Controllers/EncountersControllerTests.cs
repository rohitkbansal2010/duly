// <copyright file="EncountersControllerTests.cs" company="Duly Health and Care">
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
    public class EncountersControllerTests
    {
        private Mock<IEncounterRepository> _encounterRepositoryMocked;
        private Mock<ILogger<EncountersController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _encounterRepositoryMocked = new Mock<IEncounterRepository>();
            _loggerMocked = new Mock<ILogger<EncountersController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetEncountersForSiteByDateTest()
        {
            //Arrange
            const string siteId = "";
            DateTime date = new();
            var encounters = ConfigureRepository(siteId, date);
            var encountersController =
                new EncountersController(_encounterRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);

            encountersController.MockObjectValidator();

            //Act
            var actionResult = await encountersController.GetEncountersOfSiteByDate(siteId, date);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<Encounter>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNullOrEmpty();
            responseData.Should().HaveCount(encounters.Count());
            responseData.First().Id.Should().Be(encounters.First().Id);
        }

        private IEnumerable<Encounter> ConfigureRepository(string siteId, DateTime date)
        {
            IEnumerable<Encounter> encounters = new List<Encounter>
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _encounterRepositoryMocked
                .Setup(repository => repository.GetEncountersForSiteByDateAsync(siteId, date))
                .Returns(Task.FromResult(encounters));

            return encounters;
        }
    }
}

// <copyright file="PractitionersControllerTests.cs" company="Duly Health and Care">
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
    public class PractitionersControllerTests
    {
        private Mock<IPractitionerGeneralInfoRepository> _practitionerRepositoryMocked;
        private Mock<ILogger<PractitionersController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _practitionerRepositoryMocked = new Mock<IPractitionerGeneralInfoRepository>();
            _loggerMocked = new Mock<ILogger<PractitionersController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetPractitionersForSiteByDateTest()
        {
            //Arrange
            const string siteId = "";
            var practitioners = ConfigureRepository(siteId);
            var practitionersController =
                new PractitionersController(_practitionerRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);

            practitionersController.MockObjectValidator();

            //Act
            var actionResult = await practitionersController.GetPractitionersOfSite(siteId);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<PractitionerGeneralInfo>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNullOrEmpty();
            responseData.Should().HaveCount(practitioners.Count());
            responseData.First().Id.Should().Be(practitioners.First().Id);
        }

        private IEnumerable<PractitionerGeneralInfo> ConfigureRepository(string siteId)
        {
            IEnumerable<PractitionerGeneralInfo> practitionerGeneralInfos = new List<PractitionerGeneralInfo>
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _practitionerRepositoryMocked
                .Setup(repository => repository.GetPractitionersBySiteIdAsync(siteId))
                .Returns(Task.FromResult(practitionerGeneralInfos));

            return practitionerGeneralInfos;
        }

        [Test]
        public async Task GetPractitionersByIdentitiesTest()
        {
            //Arrange
            var identifiers = new[] { "EXTERNAL|123456" };
            var practitioners = ConfigureRepositoryForIdentities(identifiers);
            var practitionersController =
                new PractitionersController(_practitionerRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);

            practitionersController.MockObjectValidator();

            //Act
            var actionResult = await practitionersController.GetPractitionersByIdentifiers(identifiers);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<PractitionerGeneralInfo>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNullOrEmpty();
            responseData.Should().HaveCount(practitioners.Count());
            responseData.First().Id.Should().Be(practitioners.First().Id);
        }

        private IEnumerable<PractitionerGeneralInfo> ConfigureRepositoryForIdentities(string[] identifiers)
        {
            IEnumerable<PractitionerGeneralInfo> practitionerGeneralInfos = new List<PractitionerGeneralInfo>
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _practitionerRepositoryMocked
                .Setup(repository => repository.GetPractitionersByIdentifiersAsync(identifiers))
                .Returns(Task.FromResult(practitionerGeneralInfos));

            return practitionerGeneralInfos;
        }
    }
}

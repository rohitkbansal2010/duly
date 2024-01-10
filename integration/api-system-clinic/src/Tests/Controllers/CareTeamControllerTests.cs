// <copyright file="CareTeamControllerTests.cs" company="Duly Health and Care">
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.Clinic.Api.Tests.Controllers
{
    [TestFixture]
    public class CareTeamControllerTests
    {
        private Mock<ICareTeamParticipantRepository> _careTeamRepositoryMocked;
        private Mock<ILogger<CareTeamController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _careTeamRepositoryMocked = new Mock<ICareTeamParticipantRepository>();
            _loggerMocked = new Mock<ILogger<CareTeamController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetCareTeamParticipantsByEncounterIdTest()
        {
            //Arrange
            const string encounterId = "testId";
            const CareTeamStatus status = CareTeamStatus.Active;
            const CareTeamCategory category = CareTeamCategory.Longitudinal;

            var careTeamParticipants = ConfigureRepository(encounterId, status, category);

            var controller = new CareTeamController(_careTeamRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.GetCareTeamParticipantsByEncounterId(encounterId, status, category);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<CareTeamParticipant>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNullOrEmpty();
            responseData.Should().HaveCount(careTeamParticipants.Count());
            var firstItem = responseData.First();
            firstItem.MemberRole.Should().Be(careTeamParticipants.First().MemberRole);
            firstItem.Member.Should().Be(careTeamParticipants.First().Member);
        }

        [Test]
        public async Task GetCareTeamParticipantsByPatinetIdTest()
        {
            //Arrange
            const string patientId = "testId";
            const CareTeamStatus status = CareTeamStatus.Active;
            const CareTeamCategory category = CareTeamCategory.Longitudinal;

            var careTeamParticipants = ConfigureRepositoryForPatientRetrieval(patientId, status, category);

            var controller = new CareTeamController(_careTeamRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);
            controller.MockObjectValidator();

            //Act
            var actionResult = await controller.GetCareTeamParticipantsByPatientId(patientId, status, category);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<CareTeamParticipant>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNullOrEmpty();
            responseData.Should().HaveCount(careTeamParticipants.Count());
            var firstItem = responseData.First();
            firstItem.MemberRole.Should().Be(careTeamParticipants.First().MemberRole);
            firstItem.Member.Should().Be(careTeamParticipants.First().Member);
        }

        private IEnumerable<CareTeamParticipant> ConfigureRepository(string encounterId, CareTeamStatus status, CareTeamCategory category)
        {
            IEnumerable<CareTeamParticipant> careTeamParticipants = new List<CareTeamParticipant>
            {
                new ()
                {
                    Member = new PractitionerInCareTeam
                    {
                        Practitioner = new PractitionerGeneralInfo
                        {
                            Id = "testPrctId"
                        }
                    }
                }
            };

            _careTeamRepositoryMocked
                .Setup(repository => repository.GetCareTeamsParticipantsByEncounterIdAsync(encounterId, status, category))
                .Returns(Task.FromResult(careTeamParticipants));

            return careTeamParticipants;
        }

        private IEnumerable<CareTeamParticipant> ConfigureRepositoryForPatientRetrieval(string patientId, CareTeamStatus status, CareTeamCategory category)
        {
            IEnumerable<CareTeamParticipant> careTeamParticipants = new List<CareTeamParticipant>
            {
                new ()
                {
                    Member = new PractitionerInCareTeam
                    {
                        Practitioner = new PractitionerGeneralInfo
                        {
                            Id = "testPrctId"
                        }
                    }
                }
            };

            _careTeamRepositoryMocked
                .Setup(repository => repository.GetCareTeamsParticipantsByPatientIdAsync(patientId, status, category))
                .Returns(Task.FromResult(careTeamParticipants));

            return careTeamParticipants;
        }
    }
}

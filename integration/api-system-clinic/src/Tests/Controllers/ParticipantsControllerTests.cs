// <copyright file="ParticipantsControllerTests.cs" company="Duly Health and Care">
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
    public class ParticipantsControllerTests
    {
        private Mock<IParticipantRepository> _participantRepositoryMocked;
        private Mock<ILogger<ParticipantsController>> _loggerMocked;
        private Mock<IWebHostEnvironment> _iWebHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            _participantRepositoryMocked = new Mock<IParticipantRepository>();
            _loggerMocked = new Mock<ILogger<ParticipantsController>>();
            _iWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Test]
        public async Task GetParticipantsByEncounterId()
        {
            //Arrange
            const string encounterId = "";
            var participants = ConfigureRepository(encounterId);
            var participantsController = new ParticipantsController(_participantRepositoryMocked.Object, _loggerMocked.Object, _iWebHostEnvironment.Object);

            participantsController.MockObjectValidator();

            //Act
            var actionResult = await participantsController.GetParticipantsByEncounterId(encounterId);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var responseData = (IEnumerable<Participant>)((OkObjectResult)actionResult.Result).Value;
            responseData.Should().NotBeNullOrEmpty();
            responseData.Should().HaveCount(participants.Count());
            responseData.First().Person.Id.Should().Be(responseData.First().Person.Id);
        }

        private IEnumerable<Participant> ConfigureRepository(string encounterId)
        {
            IEnumerable<Participant> participants = new List<Participant>
            {
                new ()
                {
                    Person = new RelatedPersonGeneralInfo
                    {
                        Id = Guid.NewGuid().ToString()
                    }
                }
            };

            _participantRepositoryMocked
                .Setup(repository => repository.GetParticipantsByEncounterIdAsync(encounterId))
                .Returns(Task.FromResult(participants));

            return participants;
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="EncounterParticipantRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class EncounterParticipantRepositoryTests
    {
        private const string TestPractitionerID = "test_id";
        private Mock<IEncounterContext> _encounterContextMocked;
        private Mock<IEncountersClient> _clientMocked;
        private Mock<IMapper> _mapperMocked;

        [SetUp]
        public void Setup()
        {
            _encounterContextMocked = new Mock<IEncounterContext>();
            _clientMocked = new Mock<IEncountersClient>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task GetAppointmentsAsyncTest()
        {
            //Arrange
            const string encounterId = "123";
            var date = DateTime.Now;
            var epicParticipants = BuildEpicParticipants(encounterId);
            ConfigureMapper(epicParticipants);
            var _repositoryMocked = new EncounterParticipantRepository(
                _encounterContextMocked.Object,
                _clientMocked.Object,
                _mapperMocked.Object);

            //Act
            var results = await _repositoryMocked.GetParticipantsByEncounterId(encounterId);

            //Assert
            _clientMocked.Verify(x => x.ParticipantsAsync(encounterId, It.IsAny<Guid>(), default), Times.Once());
            _mapperMocked.Verify(x => x.Map<IEnumerable<Models.Participant>>(epicParticipants), Times.Once());

            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(epicParticipants.Count);
            (results.First().Person as Models.PractitionerGeneralInfo).Id.Should().Be(TestPractitionerID);
        }

        private ICollection<Participant> BuildEpicParticipants(string siteId)
        {
            ICollection<Participant> careTeamMembers = new Participant[]
            {
                new Participant
                {
                    Person = new PractitionerGeneralInfo
                    {
                        Id = TestPractitionerID
                    },
                    MemberType = MemberType.Practitioner
                }
            };

            _clientMocked
               .Setup(client => client.ParticipantsAsync(siteId, It.IsAny<Guid>(), default))
               .Returns(Task.FromResult(careTeamMembers));

            return careTeamMembers;
        }

        private void ConfigureMapper(IEnumerable<Participant> epicEncounters)
        {
            IEnumerable<Models.Participant> careTeamParticipants = new Models.Participant[]
            {
                new ()
                {
                   Person = new Models.PractitionerGeneralInfo
                   {
                       Id = TestPractitionerID
                   },
                   MemberType = Models.MemberType.Practitioner
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Models.Participant>>(epicEncounters))
                .Returns(careTeamParticipants);
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="CareTeamRepositoryTests.cs" company="Duly Health and Care">
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
    public class CareTeamRepositoryTests
    {
        private const string TestPractitionerID = "test_id";
        private const string TestPatientID = "test_patient_id";
        private Mock<IEncounterContext> _encounterContextMocked;
        private Mock<ICareTeamClient> _clientMocked;
        private Mock<IMapper> _mapperMocked;

        [SetUp]
        public void Setup()
        {
            _encounterContextMocked = new Mock<IEncounterContext>();
            _clientMocked = new Mock<ICareTeamClient>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task GetAppointmentsAsync_Test()
        {
            //Arrange
            var date = DateTime.Now;
            var epicParticipants = BuildEpicParticipants(TestPatientID);
            ConfigureMapper(epicParticipants);
            var _repositoryMocked = new CareTeamRepository(
                _encounterContextMocked.Object,
                _clientMocked.Object,
                _mapperMocked.Object);

            //Act
            var results = await _repositoryMocked.GetCareTeamParticipantsByPatientIdAsync(TestPatientID, Models.CareTeamStatus.Active, Models.CareTeamCategory.Longitudinal);

            //Assert
            _clientMocked.Verify(x => x.MembersAsync(TestPatientID, CareTeamStatus.Active, CareTeamCategory.Longitudinal, It.IsAny<Guid>(), default), Times.Once());
            _mapperMocked.Verify(x => x.Map<IEnumerable<Models.CareTeamParticipant>>(epicParticipants), Times.Once());

            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(epicParticipants.Count);
            (results.First().Member as Models.PractitionerInCareTeam).Practitioner.Id.Should().Be(TestPractitionerID);
        }

        private ICollection<CareTeamParticipant> BuildEpicParticipants(string patientId)
        {
            ICollection<CareTeamParticipant> careTeamMembers = new[]
            {
                new CareTeamParticipant
                {
                    Member = new PractitionerInCareTeam
                    {
                        Practitioner = new PractitionerGeneralInfo
                        {
                            Id = patientId
                        }
                    },
                    MemberRole = MemberRole.Practitioner
                }
            };

            _clientMocked
               .Setup(client => client.MembersAsync(patientId, CareTeamStatus.Active, CareTeamCategory.Longitudinal, It.IsAny<Guid>(), default))
               .Returns(Task.FromResult(careTeamMembers));

            return careTeamMembers;
        }

        private void ConfigureMapper(IEnumerable<CareTeamParticipant> epicEncounters)
        {
            IEnumerable<Models.CareTeamParticipant> careTeamParticipants = new Models.CareTeamParticipant[]
            {
                new ()
                {
                   Member = new Models.PractitionerInCareTeam
                   {
                       Practitioner = new Models.PractitionerGeneralInfo { Id = TestPractitionerID }
                   },
                   MemberRole = Models.MemberRole.Practitioner
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Models.CareTeamParticipant>>(epicEncounters))
                .Returns(careTeamParticipants);
        }
    }
}

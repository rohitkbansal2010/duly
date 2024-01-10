// <copyright file="PartyServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using Duly.CollaborationView.Encounter.Api.Services.Interfaces;
using Duly.Common.Security.Entities;
using Duly.Common.Security.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberType = Duly.CollaborationView.Encounter.Api.Contracts.MemberType;
using Models = Duly.CollaborationView.Encounter.Api.Repositories.Models;
using PractitionerGeneralInfo = Duly.CollaborationView.Encounter.Api.Contracts.PractitionerGeneralInfo;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class PartyServiceTests
    {
        private const string TestPatientId = "patientId";
        private static readonly string[] TestIds = { "TestId1", "TestId2" };
        private Mock<IAppointmentService> _appointmentServiceMocked;
        private Mock<IMapper> _mapperMocked;
        private Mock<ICareTeamRepository> _careTeamRepositoryMocked;
        private Mock<IActiveDirectoryAccountIdentityService> _activeDirectoryAccountIdentityMocked;

        [SetUp]
        public void SetUp()
        {
            _appointmentServiceMocked = new Mock<IAppointmentService>();
            _careTeamRepositoryMocked = new Mock<ICareTeamRepository>();
            _mapperMocked = new Mock<IMapper>();
            _activeDirectoryAccountIdentityMocked = new Mock<IActiveDirectoryAccountIdentityService>();
        }

        [Test]
        public async Task GetPractitionersBySiteIdAsync_Test()
        {
            //Arrange
            const string appointmentId = "123";

            var activeUser = BuildModelActiveUser();
            var modelsParties = BuildModelsParticipants(appointmentId);
            var modelsCareTeams = BuildModelsCareTeam(TestPatientId);
            var serviceMocked = new PartyService(
                _appointmentServiceMocked.Object,
                _careTeamRepositoryMocked.Object,
                _mapperMocked.Object,
                _activeDirectoryAccountIdentityMocked.Object);

            //Act
            var results = await serviceMocked.GetPartiesByPatientAndAppointmentIdAsync(TestPatientId, appointmentId);

            //Assert
            _appointmentServiceMocked.Verify(x => x.GetPractitionerByAppointmentIdAsync(appointmentId), Times.Once());
            _careTeamRepositoryMocked.Verify(x => x.GetCareTeamParticipantsByPatientIdAsync(TestPatientId, CareTeamStatus.Active, CareTeamCategory.Longitudinal), Times.Once());

            results = results.ToArray();
            results.Should().NotBeNullOrEmpty();
            results.Should().AllBeOfType<Party>();
            results.Should().HaveCount(modelsParties.Count() + modelsCareTeams.Count());
            results.First().Id.Should().Be(TestIds[0]);
            results.First().HumanName.FamilyName.Should().Be(activeUser.LastName);
            results.First().HumanName.GivenNames.First().Should().Be(activeUser.FirstName);
            results.ElementAt(1).Id.Should().Be(TestIds[1]);
        }

        [Test]
        public async Task GetPractitionersBySiteIdAsync_NotMatched_Test()
        {
            //Arrange
            const string appointmentId = "123";

            var activeUser = BuildModelActiveUser(appointmentId);
            var modelsParties = BuildModelsParticipants(appointmentId);
            var modelsCareTeams = BuildModelsCareTeam(TestPatientId);
            var serviceMocked = new PartyService(
                _appointmentServiceMocked.Object,
                _careTeamRepositoryMocked.Object,
                _mapperMocked.Object,
                _activeDirectoryAccountIdentityMocked.Object);

            //Act
            var results = await serviceMocked.GetPartiesByPatientAndAppointmentIdAsync(TestPatientId, appointmentId);

            //Assert
            _appointmentServiceMocked.Verify(x => x.GetPractitionerByAppointmentIdAsync(appointmentId), Times.Once());
            _careTeamRepositoryMocked.Verify(x => x.GetCareTeamParticipantsByPatientIdAsync(TestPatientId, CareTeamStatus.Active, CareTeamCategory.Longitudinal), Times.Once());

            results = results.ToArray();
            results.Should().NotBeNullOrEmpty();
            results.Should().AllBeOfType<Party>();
            results.Should().HaveCount(modelsParties.Count() + modelsCareTeams.Count() + 1);
            results.First().Id.Should().Be(activeUser.Id);
            results.First().HumanName.FamilyName.Should().Be(activeUser.LastName);
            results.First().HumanName.GivenNames.First().Should().Be(activeUser.FirstName);
            results.ElementAt(1).Id.Should().Be(TestIds[0]);
        }

        private AzureActiveDirectoryUserAccountIdentity BuildModelActiveUser(string lastName = "Bloom")
        {
            var id = Guid.NewGuid().ToString();
            var activeUser = new AzureActiveDirectoryUserAccountIdentity
            {
                Id = id,
                LastName = lastName,
                FirstName = "Bob"
            };

            _activeDirectoryAccountIdentityMocked
                .Setup(service => service.GetUserAsync())
                .Returns(() => Task.FromResult(activeUser));

            _mapperMocked
                .Setup(mapper => mapper.Map<Party>(activeUser))
                .Returns(new Party
                {
                    Id = id,
                    HumanName = new Contracts.HumanName
                    {
                        FamilyName = lastName,
                        GivenNames = new[] { "Bob" }
                    },
                    MemberType = MemberType.Another
                });

            return activeUser;
        }

        private IEnumerable<PractitionerGeneralInfo> BuildModelsParticipants(string siteId)
        {
            IEnumerable<PractitionerGeneralInfo> parties = new PractitionerGeneralInfo[]
            {
                new ()
                {
                    Id = TestIds[0]
                }
            };

            _appointmentServiceMocked
                .Setup(repo => repo.GetPractitionerByAppointmentIdAsync(siteId))
                .Returns(Task.FromResult(parties.Single()));
            _mapperMocked
                .Setup(mapper => mapper.Map<Party>(parties.Single()))
                .Returns(
                    new Party
                    {
                        Id = TestIds[0],
                        HumanName = new Contracts.HumanName
                        {
                            FamilyName = "Bloom",
                            GivenNames = new[] { "Bob" }
                        },
                        MemberType = MemberType.Doctor
                    });
            return parties;
        }

        private IEnumerable<CareTeamParticipant> BuildModelsCareTeam(string patientId)
        {
            IEnumerable<CareTeamParticipant> parties = new CareTeamParticipant[]
            {
                new ()
                {
                    Member = new PractitionerInCareTeam
                    {
                        Practitioner = new Models.PractitionerGeneralInfo
                        {
                            Id = TestIds[1]
                        }
                    }
                }
            };

            _careTeamRepositoryMocked
                .Setup(repo => repo.GetCareTeamParticipantsByPatientIdAsync(patientId, CareTeamStatus.Active, CareTeamCategory.Longitudinal))
                .Returns(Task.FromResult(parties));
            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Party>>(parties))
                .Returns(new List<Party>
                {
                    new Party
                    {
                        Id = TestIds[1],
                        HumanName = new Contracts.HumanName
                        {
                            FamilyName = "Jhonson",
                            GivenNames = new[] { "Anna", "Maria" }
                        },
                        MemberType = MemberType.Caregiver
                    }
                });

            return parties;
        }
    }
}

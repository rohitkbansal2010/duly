﻿// <copyright file="CareTeamParticipantToPartyConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;
using Attachment = Duly.CollaborationView.Encounter.Api.Repositories.Models.Attachment;
using HumanName = Duly.CollaborationView.Encounter.Api.Repositories.Models.HumanName;
using PractitionerGeneralInfo = Duly.CollaborationView.Encounter.Api.Repositories.Models.PractitionerGeneralInfo;
using Role = Duly.CollaborationView.Encounter.Api.Repositories.Models.Role;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    internal class CareTeamParticipantToPartyConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        private const string PhotoBase64Str = "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";

        [Test]
        public void ConvertTest_Practitioner_ToParty()
        {
            //Arrange
            var participantMember = new PractitionerInCareTeam
            {
                Practitioner = new PractitionerGeneralInfo
                {
                    Id = "qwerty1",
                    Names = new[]
                    {
                        new HumanName
                        {
                            FamilyName = "Reyes",
                            GivenNames = new[] { "Ana", "Maria" },
                            Prefixes = new[] { "Phd." },
                            Suffixes = new[] { "II" }
                        }
                    },
                    Photos = new[]
                    {
                        new Attachment
                        {
                            Title = "Photo",
                            ContentType = "image/x-png",
                            Data = PhotoBase64Str,
                            Size = (int)PhotoBase64Str.Length,
                            Url = "test url"
                        }
                    },
                    Roles = new[]
                    {
                        new Role { Title = "physician" }
                    }
                }
            };

            var source = new CareTeamParticipant
            {
                MemberRole = MemberRole.Practitioner,
                Member = participantMember
            };

            //Act
            var result = Mapper.Map<ApiContracts.Party>(source);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().BeEquivalentTo(participantMember.Practitioner.Id);

            result.HumanName.Should().BeOfType<ApiContracts.HumanName>();
            result.HumanName.FamilyName.Should().BeEquivalentTo(participantMember.Practitioner.Names.First().FamilyName);
            result.HumanName.GivenNames.Should().NotBeNull();
            result.HumanName.GivenNames.Should().HaveCount(participantMember.Practitioner.Names.First().GivenNames.Length);
            result.HumanName.Suffixes.Should().NotBeNull();
            result.HumanName.Suffixes.Should().HaveCount(participantMember.Practitioner.Names.First().Suffixes.Length);
            result.HumanName.Prefixes.Should().NotBeNull();
            result.HumanName.Prefixes.Should().HaveCount(1);
            result.HumanName.Prefixes[0].Should().BeEquivalentTo(participantMember.Practitioner.Names.First().Prefixes.First());

            result.Photo.Should().BeOfType<ApiContracts.Attachment>();
            result.Photo.Title.Should().BeEquivalentTo(participantMember.Practitioner.Photos.First().Title);
            result.Photo.ContentType.Should().BeEquivalentTo(participantMember.Practitioner.Photos.First().ContentType);
            result.Photo.Size.Should().Be(participantMember.Practitioner.Photos.First().Size);
            result.Photo.Data.Should().BeEquivalentTo(participantMember.Practitioner.Photos.First().Data);
            result.Photo.Url.Should().BeEquivalentTo(participantMember.Practitioner.Photos.First().Url);

            result.MemberType.Should().Be(ApiContracts.MemberType.Doctor);
        }

        [Test]
        public void ConvertTest_Throw_ServiceNotMappedException()
        {
            //Arrange
            var source = new CareTeamParticipant
            {
                MemberRole = (MemberRole)(-1),
                Member = new PractitionerInCareTeam()
                {
                }
            };

            //Act
            Action action = () => Mapper.Map<ApiContracts.Party>(source);

            //Assert
            var result = action.Should().ThrowExactly<ServiceNotMappedException>();
            result.Which.Message.Should().Be($"Unmapped MemberRole {source.MemberRole}");
        }
    }
}

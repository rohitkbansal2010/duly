// <copyright file="CareTeamParticipantConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class CareTeamParticipantConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertPractitionerWithRolesTest()
        {
            //Arrange
            PractitionerWithRoles source = new()
            {
                Resource = new R4.Practitioner
                {
                    Id = "testId3",
                    Name = new List<R4.HumanName>
                    {
                        R4.HumanName.ForFamily("family")
                    }
                },
                Roles = Array.Empty<R4.PractitionerRole>()
            };

            //Act
            var result = Mapper.Map<CareTeamParticipant>(source);

            ////Assert
            result.Should().NotBeNull();
            var member = (PractitionerInCareTeam)result.Member;
            member.Practitioner.Id.Should().Be("testId3");
            member.Practitioner.Names.First().FamilyName.Should().Be(source.Resource.Name[0].Family);
        }
    }
}

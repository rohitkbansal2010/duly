// <copyright file="CareTeamWithCompartmentsBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Builders.Implementations;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using FluentAssertions;
using Hl7.Fhir.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Builders.Implementations
{
    [TestFixture]
    public class CareTeamWithCompartmentsBuilderTests
    {
        [Test]
        public async Task ExtractCareTeamWithParticipantsAsyncTest()
        {
            //Arrange
            var endOfParticipation = DateTimeOffset.UtcNow;
            var categoryCoding = new Coding("s", "s");
            var resourceId = Guid.NewGuid().ToString();

            var aaa = "aaa";
            IEnumerable<R4.Bundle.EntryComponent> components = new R4.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new R4.CareTeam
                    {
                        Subject = new ResourceReference { Reference = Guid.NewGuid().ToString() },
                        Category = new List<CodeableConcept>()
                    }
                },
                new()
                {
                    Resource = new R4.CareTeam
                    {
                        Id = resourceId,
                        Subject = new ResourceReference
                        {
                            Reference = Guid.NewGuid().ToString()
                        },
                        Category = new List<CodeableConcept>
                        {
                            new()
                            {
                                Coding = new List<Coding>
                                {
                                    categoryCoding
                                }
                            }
                        },
                        Participant = new List<R4.CareTeam.ParticipantComponent>
                        {
                            new()
                            {
                                Member = new ResourceReference("Practitioner/aaa", aaa)
                            }
                        }
                    }
                },
                new()
                {
                    Resource = new R4.Practitioner
                    {
                        Id = aaa
                    }
                }
            };

            var mockedPractitionerWithRolesBuilder = new Mock<IPractitionerWithRolesBuilder>();
            var practitionersWithRoles = new PractitionerWithRoles[]
            {
                new()
                {
                    Resource = new() { Id = aaa }
                }
            };

            mockedPractitionerWithRolesBuilder
                .Setup(builder => builder.RetrievePractitionerWithRolesAsync(It.Is<IEnumerable<R4.Bundle.EntryComponent>>(enumerable => CheckEntryComponents(enumerable, aaa)), false))
                .ReturnsAsync(practitionersWithRoles);

            ICareTeamWithCompartmentsBuilder builder = new CareTeamWithCompartmentsBuilder(mockedPractitionerWithRolesBuilder.Object);

            //Act
            var result = await builder.ExtractCareTeamsWithParticipantsAsync(components, endOfParticipation, categoryCoding);

            //Assert
            result.Should().NotBeNull();
            result.Length.Should().Be(1);
            result.First().Resource.Should().NotBeNull();
            result.First().Resource.Id.Should().Be(resourceId);
            result.First().Practitioners.Should().NotBeEmpty();
            result.First().Practitioners.Should().HaveCount(1);
        }

        private bool CheckEntryComponents(IEnumerable<R4.Bundle.EntryComponent> components, string resourceId)
        {
            foreach (var component in components)
            {
                component.Resource.Id.Should().Be(resourceId);
            }

            return true;
        }
    }
}

// <copyright file="CareTeamWithCompartmentsAdapterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Fhir.Adapter.Adapters.Implementations;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Adapters.Implementations
{
    [TestFixture]
    public class CareTeamWithCompartmentsAdapterTests
    {
        [Test]
        public async Task FindCareTeamWithParticipantsByIdAsyncTest()
        {
            //Arrange
            var components = new R4.Bundle.EntryComponent[]
            {
                new ()
                {
                    Resource = new R4.CareTeam()
                }
            };
            const string careTeamId = "test";
            var careTeamWithParticipants = new CareTeamWithParticipants
            {
                Resource = (R4.CareTeam)components[0].Resource
            };

            var endOfParticipation = DateTimeOffset.UtcNow;
            var categoryCoding = new Coding("s", "s");

            var careTeamSearchCriteria = new CareTeamSearchCriteria
            {
                PatientReference = careTeamId,
                Status = "test",
                CategoryCoding = categoryCoding,
                EndOfParticipation = endOfParticipation
            };

            var mockedClient = new Mock<IFhirClientR4>();

            mockedClient
                .Setup(client => client.FindCareTeamsAsync(It.IsAny<SearchParams>()))
                .Returns(Task.FromResult(components));

            var mockedBuilder = new Mock<ICareTeamWithCompartmentsBuilder>();
            mockedBuilder
                .Setup(builder => builder.ExtractCareTeamsWithParticipantsAsync(components, endOfParticipation, categoryCoding, true))
                .Returns(Task.FromResult(new[] { careTeamWithParticipants }));

            ICareTeamWithCompartmentsAdapter adapter = new CareTeamWithCompartmentsAdapter(mockedClient.Object, mockedBuilder.Object);

            //Act
            var result = await adapter.FindCareTeamsWithParticipantsAsync(careTeamSearchCriteria);

            //Assert
            result.Should().NotBeNull();
            result.First().Should().Be(careTeamWithParticipants);
        }
    }
}

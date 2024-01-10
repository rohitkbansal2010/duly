// <copyright file="PastImmunizationRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

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
    public class PastImmunizationRepositoryTests
    {
        private Mock<IEncounterContext> _encounterContextMock;
        private Mock<IPatientsClient> _patientsClientMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            ConfigureEncounterContext();
            _patientsClientMock = new Mock<IPatientsClient>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetPastImmunizationsForPatientAsync_Test()
        {
            //Arrange
            var patientId = Guid.NewGuid().ToString();
            var immunizationStatuses = new[]
            {
                ImmunizationStatus.Completed, ImmunizationStatus.NotDone
            };
            var pastImmunizationStatuses = new[]
            {
                Models.PastImmunizationStatus.Completed, Models.PastImmunizationStatus.NotDone
            };

            var systemImmunizations = BuildImmunizations(patientId, immunizationStatuses);

            ConfigureMapper(systemImmunizations, pastImmunizationStatuses, immunizationStatuses);

            var repository = new PastImmunizationRepository(
                _encounterContextMock.Object,
                _mapperMock.Object,
                _patientsClientMock.Object);

            //Act
            var result = await repository.GetPastImmunizationsForPatientAsync(patientId, pastImmunizationStatuses);

            //Assert
            _patientsClientMock.Verify(
                x => x.ImmunizationsAsync(
                    patientId,
                    It.IsAny<Guid>(),
                    It.Is<IEnumerable<ImmunizationStatus>>(
                        a => a.All(s => immunizationStatuses.Contains(s))),
                    default),
                Times.Once());

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(systemImmunizations.Count);
            result.Last().Id.Should().Be(systemImmunizations.Last().Id);
        }

        private void ConfigureEncounterContext()
        {
            _encounterContextMock = new Mock<IEncounterContext>();

            _encounterContextMock
                .Setup(ctx => ctx.GetXCorrelationId())
                .Returns(Guid.NewGuid());
        }

        private ICollection<Immunization> BuildImmunizations(
            string patientId,
            ImmunizationStatus[] immunizationStatuses)
        {
            ICollection<Immunization> immunizations = new Immunization[]
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                },
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                },
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _patientsClientMock
                .Setup(client => client.ImmunizationsAsync(patientId, It.IsAny<Guid>(), immunizationStatuses, default))
                .Returns(Task.FromResult(immunizations));

            return immunizations;
        }

        private void ConfigureMapper(
            ICollection<Immunization> systemImmunizations,
            Models.PastImmunizationStatus[] pastImmunizationStatuses,
            ImmunizationStatus[] immunizationStatuses)
        {
            IEnumerable<Models.PastImmunization> pastImmunizations = systemImmunizations
                .Select(x => new Models.PastImmunization
                {
                    Id = x.Id
                });

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<Models.PastImmunization>>(systemImmunizations))
                .Returns(pastImmunizations);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<ImmunizationStatus>>(pastImmunizationStatuses))
                .Returns(immunizationStatuses);
        }
    }
}

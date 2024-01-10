// <copyright file="RecommendedImmunizationRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.Ngdp.Api.Client;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class RecommendedImmunizationRepositoryTests
    {
        private Mock<IEncounterContext> _encounterContextMocked;
        private Mock<IMapper> _mapperMocked;
        private Mock<IPatientsClient> _patientsClientMocked;
        [SetUp]
        public void SetUp()
        {
            _encounterContextMocked = new Mock<IEncounterContext>();
            _mapperMocked = new Mock<IMapper>();
            _patientsClientMocked = new Mock<IPatientsClient>();
        }

        [Test]
        public async Task GetRecommendedImmunizationsForPatientAsync_Test()
        {
            var patientId = "test-patient-id";
            RecommendedImmunizationStatus[] recommendedImmunizationStatuses =
            {
                RecommendedImmunizationStatus.DueOn,
                RecommendedImmunizationStatus.DueSoon,
                RecommendedImmunizationStatus.NotDue,
                RecommendedImmunizationStatus.Overdue,
                RecommendedImmunizationStatus.Postponed
            };
            DueStatus[] dueStatuses =
            {
                DueStatus.DueOn,
                DueStatus.DueSoon,
                DueStatus.NotDue,
                DueStatus.Overdue,
                DueStatus.Postponed
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<DueStatus>>(recommendedImmunizationStatuses))
                .Returns(dueStatuses);

            var immunizations = BuildImmunizations(patientId, dueStatuses);
            var recommendedImmunizations = new[]
            {
                new RecommendedImmunization()
                {
                    DueDate = immunizations.First().DueDate,
                    VaccineName = immunizations.First().VaccineName,
                    Status = RecommendedImmunizationStatus.DueOn,
                    Patient = new RecommendedImmunizationPatient()
                    {
                        Id = immunizations.First().Patient.Id
                    }
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<RecommendedImmunization>>(immunizations))
                .Returns(recommendedImmunizations);

            var repository = new RecommendedImmunizationRepository(
                _encounterContextMocked.Object,
                _mapperMocked.Object,
                _patientsClientMocked.Object);

            var result = await repository.GetRecommendedImmunizationsForPatientAsync(patientId, recommendedImmunizationStatuses);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(recommendedImmunizations.Length);
            result.First().VaccineName.Should().Be(recommendedImmunizations.First().VaccineName);
            result.First().Status.Should().Be(recommendedImmunizations.First().Status);
            result.First().DueDate.Should().Be(recommendedImmunizations.First().DueDate);
            result.First().Patient.Should().BeEquivalentTo(recommendedImmunizations.First().Patient);
        }

        private IEnumerable<Immunization> BuildImmunizations(string patientId, IEnumerable<DueStatus> includedStatuses)
        {
            ICollection<Immunization> immunizations = new List<Immunization>
            {
                new Immunization()
                {
                    DueDate = DateTimeOffset.Now,
                    VaccineName = "test-vac-name",
                    Status = DueStatus.DueOn,
                    Patient = new Ngdp.Api.Client.Patient()
                    {
                        Id = patientId
                    }
                }
            };
            var correlationId = Guid.NewGuid();
            _encounterContextMocked
                .Setup(context => context.GetXCorrelationId())
                .Returns(correlationId);

            _patientsClientMocked
                .Setup(client => client.ImmunizationsAsync(patientId, includedStatuses, correlationId, default))
                .Returns(Task.FromResult(immunizations));
            return immunizations;
        }
    }
}

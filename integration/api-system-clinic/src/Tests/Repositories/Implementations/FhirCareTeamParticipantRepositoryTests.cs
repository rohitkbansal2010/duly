// <copyright file="FhirCareTeamParticipantRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using AutoMapper;
using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces.Composite;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Common.Infrastructure.Exceptions;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class FhirCareTeamParticipantRepositoryTests
    {
        private const string EncounterId = "testId";
        private const string PatientReference = "testRef";

        private Mock<ICareTeamWithCompartmentsAdapter> _adapterMocked;
        private Mock<IMapper> _mapperMocked;
        private Mock<IGetFhirResourceById<R4.Encounter>> _encounterByIdMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<ICareTeamWithCompartmentsAdapter>();
            _mapperMocked = new Mock<IMapper>();
            _encounterByIdMocked = new Mock<IGetFhirResourceById<R4.Encounter>>();
        }

        [Test]
        public async Task GetCareTeamParticipantsByEncounterIdAsync_BadHttpRequestException_Test()
        {
            //Arrange
            ICareTeamParticipantRepository repository = new FhirCareTeamParticipantRepository(
                _adapterMocked.Object, _encounterByIdMocked.Object, _mapperMocked.Object);

            const CareTeamStatus status = CareTeamStatus.Active;
            const CareTeamCategory category = CareTeamCategory.Longitudinal;

            //Act
            Func<Task> action = async () => await repository.GetCareTeamsParticipantsByEncounterIdAsync(EncounterId, status, category);

            //Assert
            var exception = await action.Should().ThrowAsync<EntityNotFoundException>();
            exception.WithMessage("Encounter with ID testId was not found.");
        }

        [Test]
        public async Task GetCareTeamParticipantsByEncounterIdAsync_EntityNotFoundException_Test()
        {
            //Arrange
            ICareTeamParticipantRepository repository = new FhirCareTeamParticipantRepository(
                _adapterMocked.Object, _encounterByIdMocked.Object, _mapperMocked.Object);
            const CareTeamStatus status = CareTeamStatus.Active;
            const CareTeamCategory category = CareTeamCategory.Longitudinal;

            //Act
            Func<Task> action = async () => await repository.GetCareTeamsParticipantsByEncounterIdAsync(EncounterId, status, category);

            //Assert
            var result = await action.Should().ThrowAsync<EntityNotFoundException>();
            result.Which.Message.Should().Be("Encounter with ID testId was not found.");
        }

        [Test]
        public async Task GetCareTeamParticipantsByEncounterIdAsyncTest()
        {
            //Arrange
            const CareTeamStatus status = CareTeamStatus.Active;
            const CareTeamCategory category = CareTeamCategory.Longitudinal;
            SetUpEncounterById();
            SetUpAdapterAndMapper(status);
            ICareTeamParticipantRepository repository = new FhirCareTeamParticipantRepository(
                _adapterMocked.Object, _encounterByIdMocked.Object, _mapperMocked.Object);

            const int careTeamsResultCount = 1;
            const int numberOfCallingMapper = 1;

            //Act
            var result = await repository.GetCareTeamsParticipantsByEncounterIdAsync(EncounterId, status, category);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(careTeamsResultCount);
            _mapperMocked.Verify(
                mapper => mapper.Map<IEnumerable<CareTeamParticipant>>(It.IsAny<object>()),
                Times.Exactly(numberOfCallingMapper));
        }

        [Test]
        public async Task GetCareTeamParticipantsByPatientIdAsyncTest()
        {
            //Arrange
            const CareTeamStatus status = CareTeamStatus.Active;
            const CareTeamCategory category = CareTeamCategory.Longitudinal;
            SetUpPatientById();
            SetUpAdapterAndMapper(status);
            ICareTeamParticipantRepository repository = new FhirCareTeamParticipantRepository(
                _adapterMocked.Object, _encounterByIdMocked.Object, _mapperMocked.Object);

            const int careTeamsResultCount = 1;
            const int numberOfCallingMapper = 1;

            //Act
            var result = await repository.GetCareTeamsParticipantsByPatientIdAsync(PatientReference, status, category);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(careTeamsResultCount);
            _mapperMocked.Verify(
                mapper => mapper.Map<IEnumerable<CareTeamParticipant>>(It.IsAny<object>()),
                Times.Exactly(numberOfCallingMapper));
        }

        private void SetUpEncounterById()
        {
            _encounterByIdMocked
                .Setup(id => id.GetFhirResourceByIdAsync(EncounterId))
                .Returns(Task.FromResult(new R4.Encounter
                {
                    Subject = new Hl7.Fhir.Model.ResourceReference(PatientReference),
                }));
        }

        private void SetUpPatientById()
        {
            _encounterByIdMocked
                .Setup(id => id.GetFhirResourceByIdAsync(PatientReference))
                .Returns(Task.FromResult(new R4.Encounter
                {
                    Subject = new Hl7.Fhir.Model.ResourceReference(PatientReference),
                }));
        }

        private void SetUpAdapterAndMapper(CareTeamStatus careTeamStatus)
        {
            var careTeamWithParticipants = new CareTeamWithParticipants
            {
                Practitioners = new PractitionerWithRoles[] { new() }
            };

            _adapterMocked
                .Setup(adapter => adapter.FindCareTeamsWithParticipantsAsync(It.Is<CareTeamSearchCriteria>(criteria => CheckCareTeamSearchCriteria(careTeamStatus, criteria))))
                .Returns(Task.FromResult(new[] { careTeamWithParticipants }));

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<CareTeamParticipant>>(careTeamWithParticipants.Practitioners))
                .Returns(new List<CareTeamParticipant> { new() });
        }

        private bool CheckCareTeamSearchCriteria(CareTeamStatus careTeamStatus, CareTeamSearchCriteria criteria)
        {
            return criteria.PatientReference == PatientReference &&
                   criteria.Status == careTeamStatus.ToString() &&
                   criteria.CategoryCoding.Code == "LA28865-6" &&
                   criteria.CategoryCoding.System == "http://loinc.org" &&
                   criteria.EndOfParticipation == DateTimeOffset.UnixEpoch;
        }
    }
}

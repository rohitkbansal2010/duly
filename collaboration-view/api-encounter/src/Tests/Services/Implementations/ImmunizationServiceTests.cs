// <copyright file="ImmunizationServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Extensions;
using Duly.CollaborationView.Encounter.Api.Services.Implementations;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Implementations
{
    [TestFixture]
    public class ImmunizationServiceTests
    {
        private const string GroupNameForOtherPastImmunizations = "Other Past Immunizations";
        private const string GroupNameForCOVID19 = "COVID-19";
        private const string GroupNameForDTAP = "DTAP";
        private const string GroupNameForTDAP = "TDAP";
        public const string DUETITLE = "DUE";

        private static readonly PastImmunizationStatus[] supportedPastImmunizationStatues =
        {
            PastImmunizationStatus.Completed,
            PastImmunizationStatus.NotDone
        };

        private static readonly RecommendedImmunizationStatus[] SupportedRecommendedImmunizationStatues =
        {
            RecommendedImmunizationStatus.DueSoon,
            RecommendedImmunizationStatus.Overdue
        };

        private Mock<ICvxCodeRepository> _cvxCodeRepositoryMock;
        private Mock<IPastImmunizationRepository> _pastImmunizationRepositoryMock;
        private Mock<IRecommendedImmunizationRepository> _recommendedImmunizationRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IPatientRepository> _patientRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _cvxCodeRepositoryMock = new Mock<ICvxCodeRepository>();
            _pastImmunizationRepositoryMock = new Mock<IPastImmunizationRepository>();
            _recommendedImmunizationRepositoryMock = new Mock<IRecommendedImmunizationRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetImmunizationsByPatientIdAsyncTest_With_PastImmunizations()
        {
            //Arrange
            const string patientId = "test_patient_id";
            const string ngdpIdWithExternalPrefix = "EXTERNAL|test_ngdp_patient_id";
            var pastImmunizations = BuildPastImmunizations(patientId);
            var patient = BuildPatient(patientId, ngdpIdWithExternalPrefix);
            var ngdpPatientId = patient.Identifiers.FindIdWithExternalPrefix().SplitIdWithExternalPrefix();
            var recommendedImmunizations = BuildRecommendedImmunizations(ngdpPatientId, SupportedRecommendedImmunizationStatues);

            ConfigureMapper(pastImmunizations, recommendedImmunizations);
            var serviceMock = new ImmunizationService(
                _mapperMock.Object,
                _pastImmunizationRepositoryMock.Object,
                _recommendedImmunizationRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _cvxCodeRepositoryMock.Object);

            //Act
            var result = await serviceMock.GetImmunizationsByPatientIdAsync(patientId);

            //Assert
            _pastImmunizationRepositoryMock.Verify(
                x => x.GetPastImmunizationsForPatientAsync(
                    patientId,
                    supportedPastImmunizationStatues),
                Times.Once());

            result.Should().NotBeNull();
            result.PastImmunizations.Should().NotBeNullOrEmpty();
            result.PastImmunizations.Length.Should().Be(4);

            var firstPastImmunizationsGroup = result.PastImmunizations[0];
            var secondPastImmunizationsGroup = result.PastImmunizations[1];
            var thirdPastImmunizationsGroup = result.PastImmunizations[2];
            var fourthPastImmunizationsGroup = result.PastImmunizations[3];

            firstPastImmunizationsGroup.Vaccinations.Length.Should().Be(5);
            secondPastImmunizationsGroup.Vaccinations.Length.Should().Be(3);
            thirdPastImmunizationsGroup.Vaccinations.Length.Should().Be(1);
            fourthPastImmunizationsGroup.Vaccinations.Length.Should().Be(1);
            thirdPastImmunizationsGroup.Vaccinations[0].Date.Should().BeNull();
            fourthPastImmunizationsGroup.Vaccinations[0].Date.Should().BeNull();

            firstPastImmunizationsGroup.Title.Should().Be(GroupNameForOtherPastImmunizations);
            secondPastImmunizationsGroup.Title.Should().Be(GroupNameForCOVID19);
            thirdPastImmunizationsGroup.Title.Should().Be(GroupNameForDTAP);
            fourthPastImmunizationsGroup.Title.Should().Be(GroupNameForTDAP);

            firstPastImmunizationsGroup.Vaccinations[0].Date.Value.Should()
                .BeAfter(secondPastImmunizationsGroup.Vaccinations[0].Date.Value);

            firstPastImmunizationsGroup.Vaccinations[1].Date.Value.Should()
                .BeAfter(firstPastImmunizationsGroup.Vaccinations[2].Date.Value);

            firstPastImmunizationsGroup.Vaccinations.Last().Title.Should().Be("Vaccine_3");
            secondPastImmunizationsGroup.Vaccinations.Last().Title.Should().Be("Vaccine_5");

            result.RecommendedImmunizations.Should().NotBeEmpty();
            result.RecommendedImmunizations.First().Title.Should().Be("Covid-19");
            result.RecommendedImmunizations.First().Vaccinations.Should().NotBeEmpty();
            result.RecommendedImmunizations.First().Vaccinations.First().Title.Should().Be("Covid-19 Vaccine Pfizer 30 mcg/0.3 ml");
            result.RecommendedImmunizations.First().Vaccinations.First().DateTitle.Should().Be(DUETITLE);
            result.RecommendedImmunizations.First().Vaccinations.First().Status.Should().Be(ApiContracts.RecommendedVaccinationStatus.DueOn);
            result.RecommendedImmunizations.First().Vaccinations.First().Date.Should().Be(new DateTimeOffset(DateTime.SpecifyKind(new DateTime(2022, 2, 20), DateTimeKind.Utc)));
        }

        [Test]
        public async Task GetImmunizationsByPatientIdAsyncTest_With_PastImmunizations_WithoutCvxCodeVaccineGroupNameDictionary()
        {
            //Arrange
            const string patientId = "test_patient_id";
            const string ngdpIdWithExternalPrefix = "EXTERNAL|test_ngdp_patient_id";
            var pastImmunizations = BuildPastImmunizations(patientId, true);
            var patient = BuildPatient(patientId, ngdpIdWithExternalPrefix);
            var ngdpPatientId = patient.Identifiers.FindIdWithExternalPrefix().SplitIdWithExternalPrefix();
            var recommendedImmunizations = BuildRecommendedImmunizations(ngdpPatientId, SupportedRecommendedImmunizationStatues);

            ConfigureMapper(pastImmunizations, recommendedImmunizations);
            var serviceMock = new ImmunizationService(
                _mapperMock.Object,
                _pastImmunizationRepositoryMock.Object,
                _recommendedImmunizationRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _cvxCodeRepositoryMock.Object);

            //Act
            var result = await serviceMock.GetImmunizationsByPatientIdAsync(patientId);

            //Assert
            _pastImmunizationRepositoryMock.Verify(
                x => x.GetPastImmunizationsForPatientAsync(
                    patientId,
                    supportedPastImmunizationStatues),
                Times.Once());

            result.Should().NotBeNull();
            result.PastImmunizations.Should().NotBeNullOrEmpty();
            result.PastImmunizations.Length.Should().Be(1);
        }

        private IEnumerable<PastImmunization> BuildPastImmunizations(
            string patientId,
            bool withoutCvxCodeVaccineGroupNameDictionary = false)
        {
            var pastImmunizations = new List<PastImmunization>();

            var baseDate = new DateTime(2021, 1, 3);
            for (var i = 1; i <= 10; i++)
            {
                DateTime? dateTime = i switch
                {
                    < 5 and >= 1 => baseDate.AddDays(i),
                    _ => null
                };

                var vaccine = new PastImmunizationVaccine
                {
                    Text = $"Vaccine_{11 - i}",
                };

                if (i % 3 == 0)
                {
                    // For "COVID-19"
                    vaccine.CvxCodes = new[] { "208" };
                }

                if (i == 5)
                {
                    // For "TDAP"
                    vaccine.CvxCodes = new[] { "115" };
                }

                if (i == 7)
                {
                    // For "DTAP"
                    vaccine.CvxCodes = new[] { "110" };
                }

                if (i == 8)
                {
                    vaccine.CvxCodes = new[] { "not_exist" };
                }

                pastImmunizations.Add(new PastImmunization
                {
                    Id = i.ToString(),
                    Vaccine = vaccine,
                    Status = i % 2 == 0 ? PastImmunizationStatus.NotDone : PastImmunizationStatus.Completed,
                    OccurrenceDateTime = dateTime
                });
            }

            var cvxCodeVaccineGroupNameDictionary = default(Dictionary<string, string>);
            if (!withoutCvxCodeVaccineGroupNameDictionary)
            {
                cvxCodeVaccineGroupNameDictionary = new Dictionary<string, string>
                {
                    { "110", "DTAP" },
                    { "115", "TDAP" },
                    { "197", "FLU" },
                    { "208", "COVID-19" }
                };
            }

            _cvxCodeRepositoryMock
                .Setup(r => r.FindVaccineGroupNamesByCodesAsync(It.IsAny<string[]>()))
                .Returns(Task.FromResult((IReadOnlyDictionary<string, string>)cvxCodeVaccineGroupNameDictionary));

            _pastImmunizationRepositoryMock
                .Setup(
                    repo => repo.GetPastImmunizationsForPatientAsync(
                        patientId,
                        supportedPastImmunizationStatues))
                .Returns(Task.FromResult(pastImmunizations.AsEnumerable()));

            return pastImmunizations;
        }

        private void ConfigureMapper(IEnumerable<PastImmunization> pastImmunizations, IEnumerable<RecommendedImmunization> recommendedImmunizations)
        {
            foreach (var pastImmunization in pastImmunizations)
            {
                var vaccination = new ApiContracts.Vaccination()
                {
                    Title = pastImmunization.Vaccine.Text,
                    Date = pastImmunization.OccurrenceDateTime
                };

                _mapperMock
                    .Setup(mapper => mapper.Map<ApiContracts.Vaccination>(pastImmunization))
                    .Returns(vaccination);
            }

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<ApiContracts.RecommendedVaccinationStatus>(recommendedImmunizations.First().Status))
                .Returns(ApiContracts.RecommendedVaccinationStatus.DueOn);

            var immunizationsRecommendedGroup = new[]
            {
                new ApiContracts.ImmunizationsRecommendedGroup()
                {
                    Title = recommendedImmunizations.First().VaccineName,
                    Vaccinations = new[]
                    {
                        new ApiContracts.RecommendedVaccination()
                        {
                            Date = recommendedImmunizations.First().DueDate,
                            Title = "Covid-19 Vaccine Pfizer 30 mcg/0.3 ml",
                            Status = ApiContracts.RecommendedVaccinationStatus.DueOn,
                            DateTitle = DUETITLE
                        }
                    }
                }
            };
            _mapperMock
                .Setup(mapper => mapper.Map<ApiContracts.ImmunizationsRecommendedGroup[]>(recommendedImmunizations))
                .Returns(immunizationsRecommendedGroup);
        }

        private Patient BuildPatient(string patientId, string ngdpIdWithExternalPrefix)
        {
            var patient = new Patient()
            {
                Identifiers = new[] { ngdpIdWithExternalPrefix }
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetPatientByIdAsync(patientId))
                .Returns(Task.FromResult(patient));

            return patient;
        }

        private IEnumerable<RecommendedImmunization> BuildRecommendedImmunizations(string ngdpId, RecommendedImmunizationStatus[] statuses)
        {
            var recommendedImmunizations = new[]
            {
                new RecommendedImmunization()
                {
                    Patient = new RecommendedImmunizationPatient() { Id = ngdpId },
                    VaccineName = "Covid-19",
                    DueDate = new DateTimeOffset(DateTime.SpecifyKind(new DateTime(2022, 2, 20), DateTimeKind.Utc)),
                    Status = RecommendedImmunizationStatus.DueOn
                }
            };

            _recommendedImmunizationRepositoryMock
                .Setup(repo => repo.GetRecommendedImmunizationsForPatientAsync(ngdpId, statuses))
                .Returns(Task.FromResult(recommendedImmunizations.AsEnumerable()));

            return recommendedImmunizations;
        }
    }
}
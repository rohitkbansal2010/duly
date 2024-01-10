// <copyright file="FhirAllergyIntoleranceRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using AutoMapper;
using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using Duly.Clinic.Fhir.Adapter.Extensions;
using FluentAssertions;
using Hl7.Fhir.Model;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class FhirAllergyIntoleranceRepositoryTests
    {
        private const string CodingSystem = "http://terminology.hl7.org/CodeSystem/allergyintolerance-verification";

        /// <summary>
        /// VerificationStatus: unconfirmed | confirmed | refuted | entered-in-error.
        /// </summary>
        private const string VerificationConfirmed = "confirmed";
        private const string VerificationUnconfirmed = "unconfirmed";
        private const string VerificationRefuted = "refuted";
        private const string VerificationEnteredInError = "entered-in-error";

        private const string IdForNormal = "id-normal";
        private const string IdForWithoutCategory = "id-without-category";
        private const string IdForWithoutReaction = "id-without-reaction";
        private const string IdForWithoutRecordedDate = "id-without-recorded-date";

        private Mock<IMapper> _mapperMocked;
        private Mock<IAllergyIntoleranceAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IAllergyIntoleranceAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [TestCase(VerificationConfirmed)]
        [TestCase(VerificationUnconfirmed)]
        [TestCase(VerificationRefuted)]
        [TestCase(VerificationEnteredInError)]
        public async Task GetConfirmedAllergyIntoleranceForPatientAsync_ClinicalStatus_Active_Test(string verificationStatus)
        {
            //Arrange
            const string patientId = "testId";
            var r4AllergyIntolerance = SetUpAdapter(verificationStatus);
            var expectedAllergyIntolerance = SetUpMapper(r4AllergyIntolerance);

            IAllergyIntoleranceRepository repository = new FhirAllergyIntoleranceRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var results = await repository.GetConfirmedAllergyIntoleranceForPatientAsync(patientId, ClinicalStatus.Active);

            //Assert
            _adapterMocked.Verify(
                x => x.FindAllergyIntolerancesAsync(
                    It.Is<SearchCriteria>(
                        p => p.PatientId == patientId
                             && p.Status == ClinicalStatus.Active.ToString())),
                Times.Once());

            _mapperMocked.Verify(
                x => x.Map<IEnumerable<AllergyIntolerance>>(
                    It.Is<IEnumerable<R4.AllergyIntolerance>>(
                        p => p.Count() == expectedAllergyIntolerance.Length
                             && p.Any(o => o.VerificationStatus.Coding.Contains(new Coding(CodingSystem, VerificationConfirmed), new CodingsComparer()))
                             && p.Any(o => o.RecordedDate != null))),
                Times.Once());

            results.Should().NotBeNull();
            results.Should().HaveCount(expectedAllergyIntolerance.Length);
            if (expectedAllergyIntolerance.Any())
            {
                results.First().Id.Should().Be(IdForNormal);
            }
        }

        private static IEnumerable<R4.AllergyIntolerance> GetConfirmedAndValid(IEnumerable<R4.AllergyIntolerance> r4AllergyIntolerance)
        {
            var codingVerificationStatus = new Coding(CodingSystem, VerificationConfirmed);

            var validR4AllergyIntolerance = r4AllergyIntolerance
                .Where(x => x.VerificationStatus.Coding.Contains(codingVerificationStatus, new CodingsComparer()))
                .Where(x => x.RecordedDate != null);

            return validR4AllergyIntolerance;
        }

        private static CodeableConcept BuildVerificationStatus(string verificationStatus)
        {
            return new CodeableConcept
            {
                Coding = new List<Coding> { new(CodingSystem, verificationStatus) }
            };
        }

        private static List<R4.AllergyIntolerance.AllergyIntoleranceCategory?> BuildCategory()
        {
            return new List<R4.AllergyIntolerance.AllergyIntoleranceCategory?>
            {
                R4.AllergyIntolerance.AllergyIntoleranceCategory.Medication
            };
        }

        private static List<R4.AllergyIntolerance.ReactionComponent> BuildReactionComponent()
        {
            return new List<R4.AllergyIntolerance.ReactionComponent>
            {
                new()
                {
                    Description = "The name of the reaction.",
                    Severity = R4.AllergyIntolerance.AllergyIntoleranceSeverity.Severe
                }
            };
        }

        private IEnumerable<R4.AllergyIntolerance> SetUpAdapter(string verificationStatus)
        {
            IEnumerable<R4.AllergyIntolerance> results = new R4.AllergyIntolerance[]
            {
                new()
                {
                    Id = IdForNormal,
                    VerificationStatus = BuildVerificationStatus(VerificationConfirmed),
                    RecordedDateElement = FhirDateTime.Now(),
                    Code = new CodeableConcept("system", "code", "Allergen name"),
                    Reaction = BuildReactionComponent(),
                    Category = BuildCategory()
                },
                new()
                {
                    Id = IdForWithoutCategory,
                    VerificationStatus = BuildVerificationStatus(VerificationConfirmed),
                    RecordedDateElement = FhirDateTime.Now(),
                    Code = new CodeableConcept("system", "code", "Allergen name"),
                    Reaction = BuildReactionComponent()
                },
                new()
                {
                    Id = IdForWithoutReaction,
                    VerificationStatus = BuildVerificationStatus(VerificationConfirmed),
                    RecordedDateElement = FhirDateTime.Now(),
                    Code = new CodeableConcept("system", "code", "Allergen name"),
                    Category = BuildCategory()
                },
                new()
                {
                    Id = IdForWithoutRecordedDate,
                    VerificationStatus = BuildVerificationStatus(verificationStatus),
                    Code = new CodeableConcept("system", "code", "Allergen name"),
                    Reaction = BuildReactionComponent(),
                    Category = BuildCategory()
                }
            };

            _adapterMocked
                .Setup(adapter => adapter.FindAllergyIntolerancesAsync(It.IsAny<SearchCriteria>()))
                .Returns(Task.FromResult(results));

            return results;
        }

        private AllergyIntolerance[] SetUpMapper(IEnumerable<R4.AllergyIntolerance> r4AllergyIntolerance)
        {
            var results = GetConfirmedAndValid(r4AllergyIntolerance)
                .Select(item => new AllergyIntolerance
                {
                    Id = item.Id
                })
                .ToArray();

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<AllergyIntolerance>>(It.IsAny<IEnumerable<R4.AllergyIntolerance>>()))
                .Returns(results);

            return results;
        }
    }
}

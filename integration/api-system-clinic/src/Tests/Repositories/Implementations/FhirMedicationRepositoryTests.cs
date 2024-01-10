// <copyright file="FhirMedicationRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias stu3;

using AutoMapper;
using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class FhirMedicationRepositoryTests
    {
        private Mock<IMapper> _mapperMocked;
        private Mock<IMedicationStatementWithCompartmentsAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IMedicationStatementWithCompartmentsAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [TestCase(null, null)]
        [TestCase(MedicationStatus.Active, null)]
        [TestCase(null, new MedicationCategory[0])]
        [TestCase(null, new[] { MedicationCategory.Community })]
        [TestCase(null, new[] { MedicationCategory.Community, MedicationCategory.PatientSpecified })]
        public async Task FindMedicationsForPatientAsyncTest(MedicationStatus? medicationStatus, MedicationCategory[] medicationCategories)
        {
            //Arrange
            const string patientId = "testId";
            MedicationSearchCriteria actualParameters = null;
            var medications = SetUpAdapter(msc => actualParameters = msc);
            var expectedMedications = SetUpMapper(medications);
            IMedicationRepository repository = new FhirMedicationRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var results = await repository.FindMedicationsForPatientAsync(patientId, medicationStatus, medicationCategories);

            //Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(expectedMedications.Count());
            results.First().Id.Should().Be(expectedMedications.First().Id);
            results.First().Status.Should().Be(expectedMedications.First().Status);
            results.First().Reason.Should().Be(expectedMedications.First().Reason);

            actualParameters.Should().NotBeNull();
            actualParameters.Status.Should().Be(medicationStatus?.ToString());

            actualParameters.Categories.Should().BeEquivalentTo(medicationCategories?.Select(mc => mc.ToString()));
        }

        private IEnumerable<MedicationStatementWithCompartments> SetUpAdapter(Action<MedicationSearchCriteria> adapterCallback)
        {
            IEnumerable<MedicationStatementWithCompartments> medications = new MedicationStatementWithCompartments[]
            {
                new()
                {
                    Resource = new STU3.MedicationStatement()
                }
            };

            _adapterMocked
                .Setup(adapter => adapter.FindMedicationsWithCompartmentsAsync(It.IsAny<MedicationSearchCriteria>()))
                .Callback(adapterCallback)
                .Returns(Task.FromResult(medications));

            return medications;
        }

        private IEnumerable<Medication> SetUpMapper(IEnumerable<MedicationStatementWithCompartments> medications)
        {
            IEnumerable<Medication> medicationsTarget = new Medication[]
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = MedicationStatus.Active,
                    Reason = new MedicationReason()
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Medication>>(medications))
                .Returns(medicationsTarget);

            return medicationsTarget;
        }
    }
}
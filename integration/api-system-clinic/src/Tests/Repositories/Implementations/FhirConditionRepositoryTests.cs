// <copyright file="FhirConditionRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using AutoMapper;
using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Api.Repositories.Interfaces;
using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Adapters.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts.SearchCriteria;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Implementations
{
    [TestFixture]
    public class FhirConditionRepositoryTests
    {
        private const string ProblemCategory = "problem-list-item";

        private const string IdForActive = "id-active";
        private const string IdForInactive = "id-inactive";
        private const string IdForResolved = "id-resolved";

        private Mock<IMapper> _mapperMocked;
        private Mock<IHealthConditionAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IHealthConditionAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [Test]
        public async Task FindProblemsForPatientAsync_Test()
        {
            //Arrange
            const string patientId = "testId";
            ConditionClinicalStatus[] clinicalStatusArray = new[]
            {
                ConditionClinicalStatus.Active,
                ConditionClinicalStatus.Inactive,
                ConditionClinicalStatus.Resolved
            };

            var r4Conditions = SetUpAdapter(clinicalStatusArray);
            var expectedCondition = SetUpMapper(r4Conditions);

            IConditionRepository repository = new FhirConditionRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var results = await repository.FindProblemsForPatientAsync(patientId, clinicalStatusArray);

            //Assert
            _adapterMocked.Verify(
                x => x.FindConditionsForPatientAsync(
                    It.Is<ConditionSearchCriteria>(
                        p => p.PatientId == patientId
                             && p.Categories.Length == 1 && p.Categories[0] == ProblemCategory
                             && p.Statuses.Length == clinicalStatusArray.Length
                             && p.Statuses[0] == R4.Condition.ConditionClinicalStatusCodes.Active
                             && p.Statuses[1] == R4.Condition.ConditionClinicalStatusCodes.Inactive
                             && p.Statuses[2] == R4.Condition.ConditionClinicalStatusCodes.Resolved)),
                Times.Once());

            _mapperMocked.Verify(
                x => x.Map<IEnumerable<Condition>>(It.IsAny<IEnumerable<R4.Condition>>()),
                Times.Once());

            results.Should().NotBeNull();
            results.Should().HaveCount(expectedCondition.Length);
            results.First().Id.Should().Be(IdForActive);
            results.Last().Id.Should().Be(IdForResolved);
        }

        [Test]
        public void FindProblemsForPatientAsync_ClinicalStatus_Exception()
        {
            //Arrange
            const string patientId = "testId";
            ConditionClinicalStatus[] clinicalStatusArray = new ConditionClinicalStatus[]
            {
                (ConditionClinicalStatus)(-1)
            };

            IConditionRepository repository = new FhirConditionRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            Action action = () => repository
                .FindProblemsForPatientAsync(patientId, clinicalStatusArray)
                .GetAwaiter()
                .GetResult();

            //Assert
            var result = action.Should().ThrowExactly<ConceptNotMappedException>();
            result.Which.Message.Should().Be("Missing mapping for ConditionClinicalStatus");
        }

        private IEnumerable<R4.Condition> SetUpAdapter(ConditionClinicalStatus[] clinicalStatusArray)
        {
            IEnumerable<R4.Condition> results = new R4.Condition[]
            {
                new R4.Condition
                {
                    Id = IdForActive
                },
                new R4.Condition
                {
                    Id = IdForInactive
                },
                new R4.Condition
                {
                    Id = IdForResolved
                }
            };

            _adapterMocked
                .Setup(adapter => adapter.FindConditionsForPatientAsync(It.IsAny<ConditionSearchCriteria>()))
                .Returns(Task.FromResult(results));

            return results;
        }

        private Condition[] SetUpMapper(
            IEnumerable<R4.Condition> r4Condition)
        {
            var results = r4Condition
                .Select(item => new Condition
                {
                    Id = item.Id
                })
                .ToArray();

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Condition>>(It.IsAny<IEnumerable<R4.Condition>>()))
                .Returns(results);

            return results;
        }
    }
}

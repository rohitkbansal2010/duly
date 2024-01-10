// <copyright file="FhirImmunizationRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using AutoMapper;
using Duly.Clinic.Api.Repositories.Implementations;
using Duly.Clinic.Api.Repositories.Interfaces;
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
    public class FhirImmunizationRepositoryTests
    {
        private Mock<IMapper> _mapperMocked;
        private Mock<IImmunizationAdapter> _adapterMocked;

        [SetUp]
        public void SetUp()
        {
            _adapterMocked = new Mock<IImmunizationAdapter>();
            _mapperMocked = new Mock<IMapper>();
        }

        [TestCase(null, null)]
        [TestCase(new ImmunizationStatus[0], new string[0])]
        [TestCase(new[] { ImmunizationStatus.Completed }, new[] { "completed" })]
        [TestCase(new[] { ImmunizationStatus.Completed, ImmunizationStatus.NotDone, ImmunizationStatus.EnteredInError }, new[] { "completed", "not-done", "entered-in-error" })]
        public async Task FindImmunizationsForPatientAsyncTest(ImmunizationStatus[] immunizationStatuses, string[] expectedFhirImmunizationStatuses)
        {
            //Arrange
            const string patientId = "testId";
            ImmunizationSearchCriteria actualParameters = null;
            var immunizations = SetUpAdapter(msc => actualParameters = msc);
            var expectedImmunizations = SetUpMapper(immunizations);
            IImmunizationRepository repository = new FhirImmunizationRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var results = await repository.FindImmunizationsForPatientAsync(patientId, immunizationStatuses);

            //Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(expectedImmunizations.Count());

            results.First().Id.Should().Be(expectedImmunizations.First().Id);
            results.First().Status.Should().Be(expectedImmunizations.First().Status);

            results.First().Vaccine.Should().NotBeNull();
            results.First().Vaccine.Text.Should().Be(expectedImmunizations.First().Vaccine.Text);
            results.First().Vaccine.CvxCodes.Should().NotBeNullOrEmpty();
            results.First().Vaccine.CvxCodes.Should().HaveCount(expectedImmunizations.First().Vaccine.CvxCodes.Length);
            results.First().Vaccine.CvxCodes.First().Should().Be(expectedImmunizations.First().Vaccine.CvxCodes.First());

            actualParameters.Should().NotBeNull();
            actualParameters.Statuses.Should().BeEquivalentTo(expectedFhirImmunizationStatuses);
        }

        [Test]
        public async Task WrongStatus_ThrowsException_Test()
        {
            //Arrange
            var patientId = It.IsAny<string>();
            var status = new[]
            {
                (ImmunizationStatus)999
            };

            IImmunizationRepository repository = new FhirImmunizationRepository(_adapterMocked.Object, _mapperMocked.Object);

            //Act
            var result = status.Invoking(async s => await repository.FindImmunizationsForPatientAsync(patientId, s));

            //Assert
            await result.Should()
                .ThrowAsync<ArgumentOutOfRangeException>()
                .WithParameterName("immunizationStatus");
        }

        private IEnumerable<R4.Immunization> SetUpAdapter(Action<ImmunizationSearchCriteria> adapterCallback)
        {
            IEnumerable<R4.Immunization> immunizations = new[]
            {
                new R4.Immunization()
            };

            _adapterMocked
                .Setup(adapter => adapter.FindImmunizationsForPatientAsync(It.IsAny<ImmunizationSearchCriteria>()))
                .Callback(adapterCallback)
                .Returns(Task.FromResult(immunizations));

            return immunizations;
        }

        private IEnumerable<Immunization> SetUpMapper(IEnumerable<R4.Immunization> immunizations)
        {
            IEnumerable<Immunization> immunizationsTarget = new[]
            {
                new Immunization
                {
                    Id = Guid.NewGuid().ToString(),
                    Vaccine = new()
                    {
                        CvxCodes = new[] { "44" },
                        Text = "COVID-19"
                    }
                }
            };

            _mapperMocked
                .Setup(mapper => mapper.Map<IEnumerable<Immunization>>(immunizations))
                .Returns(immunizationsTarget);

            return immunizationsTarget;
        }
    }
}
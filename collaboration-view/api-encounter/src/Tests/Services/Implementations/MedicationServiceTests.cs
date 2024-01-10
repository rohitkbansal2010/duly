// <copyright file="MedicationServiceTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
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
    public class MedicationServiceTests
    {
        private Mock<IMedicationRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IMedicationRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Test]
        public async Task GetMedicationByIdAsyncTest()
        {
            //Arrange
            const string patientId = "123";
            var medications = BuildMedications(patientId);
            ConfigureMapper(medications);
            var serviceMock = new MedicationService(
                _mapperMock.Object,
                _repositoryMock.Object);

            //Act
            var result = await serviceMock.GetMedicationsByPatientIdAsync(patientId);

            //Assert
            _repositoryMock.Verify(
                x => x.GetMedicationsAsync(
                    patientId,
                    MedicationStatus.Active,
                    new[] { MedicationCategory.Community, MedicationCategory.PatientSpecified }),
                Times.Once());

            result.Should().NotBeNull();
            result.Should().BeOfType<ApiContracts.Medications>();

            result.Regular.Should().NotBeNullOrEmpty();
            result.Regular.Should().AllBeOfType<ApiContracts.Medication>();
            result.Regular.Should().HaveCount(10);
            result.Regular.Where(x => x.ScheduleType == ApiContracts.MedicationScheduleType.Regular).Should().HaveCount(10);
            result.Regular[0].Id.Should().Be("5");
            result.Regular[4].Id.Should().Be("1");
            result.Regular[5].Id.Should().Be("6");
            result.Regular[9].Id.Should().Be("10");

            result.Other.Should().NotBeNullOrEmpty();
            result.Other.Where(x => x.ScheduleType == ApiContracts.MedicationScheduleType.Other).Should().HaveCount(10);
            result.Other.Should().AllBeOfType<ApiContracts.Medication>();
            result.Other.Should().HaveCount(10);
            result.Other[0].Id.Should().Be("15");
            result.Other[4].Id.Should().Be("11");
            result.Other[5].Id.Should().Be("16");
            result.Other[9].Id.Should().Be("20");
        }

        private IEnumerable<Medication> BuildMedications(string patientId)
        {
            var medications = new List<Medication>();

            var baseDate = new DateTime(2021, 7, 1);
            for (var i = 1; i <= 20; i++)
            {
                DateTime? startDate = i switch
                {
                    < 6 and >= 1 => baseDate.AddDays(i),
                    < 11 and >= 6 => null,
                    < 16 and >= 10 => baseDate.AddDays(i),
                    < 21 and >= 16 => null,
                    _ => null
                };

                medications.Add(new Medication
                {
                    Id = i.ToString(),
                    Status = MedicationStatus.Active,
                    Period = new Period
                    {
                        Start = startDate.HasValue ? new DateTimeOffset(startDate.Value) : null
                    }
                });
            }

            _repositoryMock
                .Setup(
                    repo => repo.GetMedicationsAsync(
                        patientId,
                        MedicationStatus.Active,
                        new[] { MedicationCategory.Community, MedicationCategory.PatientSpecified }))
                .Returns(Task.FromResult(medications.AsEnumerable()));

            return medications;
        }

        private void ConfigureMapper(IEnumerable<Medication> medications)
        {
            var apiMedications =
                medications.Select(x => new ApiContracts.Medication
                {
                    Id = x.Id,
                    ScheduleType = int.Parse(x.Id) <= 10
                        ? ApiContracts.MedicationScheduleType.Regular
                        : ApiContracts.MedicationScheduleType.Other,
                    Title = $"Medicine_{int.Parse(x.Id):00}",
                    StartDate = x.Period.Start?.Date
                }).ToArray();

            _mapperMock
                .Setup(mapper => mapper.Map<ApiContracts.Medication[]>(medications))
                .Returns(apiMedications);
        }
    }
}
// <copyright file="MedicationRepositoryTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using AutoMapper;
using Duly.Clinic.Api.Client;
using Duly.CollaborationView.Encounter.Api.Contexts.Interfaces;
using Duly.CollaborationView.Encounter.Api.Repositories.Implementations;
using Duly.CollaborationView.Encounter.Api.Repositories.Interfaces;
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
    internal class MedicationRepositoryTests
    {
        private Mock<IEncounterContext> _encounterContextMock;
        private Mock<IPatientsClient> _clientMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IPatientIdClient> _patientIdClient;

        [SetUp]
        public void SetUp()
        {
            ConfigureEncounterContext();
            _clientMock = new Mock<IPatientsClient>();
            _mapperMock = new Mock<IMapper>();
            _patientIdClient = new Mock<IPatientIdClient>();
        }

        [Test]
        public async Task GetMedicationsAsyncTest_with_MedicationStatus()
        {
            //Arrange
            const string patientId = "123";
            MedicationStatus? epicMedicationStatus = MedicationStatus.Active;
            Models.MedicationStatus medicationStatus = Models.MedicationStatus.Active;
            MedicationCategory[] epicMedicationCategories = Array.Empty<MedicationCategory>();
            Models.MedicationCategory[] medicationCategories = Array.Empty<Models.MedicationCategory>();
            var epicMedications = BuildEpicMedications(patientId, epicMedicationStatus, epicMedicationCategories);
            ConfigureMapper(epicMedications, medicationStatus, medicationCategories);
            IMedicationRepository repositoryMock = new MedicationRepository(
                _encounterContextMock.Object,
                _mapperMock.Object,
                _clientMock.Object,
                _patientIdClient.Object);

            //Act
            var results = (await repositoryMock.GetMedicationsAsync(patientId, medicationStatus, medicationCategories)).ToList();

            //Assert
            _mapperMock.Verify(
                x => x.Map<MedicationStatus>(medicationStatus),
                Times.Once());

            _clientMock.Verify(
                x => x.MedicationsAsync(patientId, It.IsAny<Guid>(), epicMedicationStatus, epicMedicationCategories, default),
                Times.Once());

            results.Should().NotBeNullOrEmpty();
            results.Should().HaveCount(epicMedications.Count);
            results.First().Id.Should().Be(epicMedications.First().Id);
        }

        private void ConfigureEncounterContext()
        {
            _encounterContextMock = new Mock<IEncounterContext>();

            _encounterContextMock
                .Setup(ctx => ctx.GetXCorrelationId())
                .Returns(Guid.NewGuid());
        }

        private ICollection<Medication> BuildEpicMedications(string patientId, MedicationStatus? epicMedicationStatus, MedicationCategory[] epicMedicationCategories)
        {
            ICollection<Medication> medications = new Medication[]
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            _clientMock
                .Setup(client => client.MedicationsAsync(patientId, It.IsAny<Guid>(), epicMedicationStatus, epicMedicationCategories, default))
                .Returns(Task.FromResult(medications));

            return medications;
        }

        private void ConfigureMapper(IEnumerable<Medication> epicMedications, Models.MedicationStatus? medicationStatus, Models.MedicationCategory[] medicationCategories)
        {
            IEnumerable<Models.Medication> medications = new Models.Medication[]
            {
                new ()
                {
                    Id = epicMedications.First().Id
                }
            };

            IEnumerable<MedicationCategory> defaultMedicationCategoriesMapped = new[]
            {
               MedicationCategory.Community,
               MedicationCategory.PatientSpecified
            };

            if (medicationStatus.HasValue)
            {
                _mapperMock
                    .Setup(mapper => mapper.Map<MedicationStatus>(medicationStatus.Value))
                    .Returns(medicationStatus.Value == Models.MedicationStatus.Active
                        ? MedicationStatus.Active
                        : MedicationStatus.Inactive);
            }

            if (medicationCategories == null)
            {
                _mapperMock
                    .Setup(mapper => mapper.Map<IEnumerable<MedicationCategory>>(medicationCategories))
                    .Returns<IEnumerable<MedicationCategory>>(null);
            }
            else if (medicationCategories.Any())
            {
                _mapperMock
                    .Setup(mapper => mapper.Map<IEnumerable<MedicationCategory>>(medicationCategories))
                    .Returns(defaultMedicationCategoriesMapped);
            }

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<Models.Medication>>(epicMedications))
                .Returns(medications);
        }

        [TestCase(null, null, new[] { "inpatient", "outpatient", "community", "specific" })]
        [TestCase(new MedicationCategory[] { }, new Models.MedicationCategory[] { }, new string[] { })]
        [TestCase(
            new[] { MedicationCategory.Inpatient, MedicationCategory.Community, MedicationCategory.PatientSpecified },
            new[] { Models.MedicationCategory.Inpatient, Models.MedicationCategory.Community, Models.MedicationCategory.PatientSpecified },
            new[] { "inpatient", "community", "specific" })]
        public async Task GetMedicationsAsyncTest_with_MedicationCategories(MedicationCategory[] epicMedicationCategories, Models.MedicationCategory[] medicationCategories, string[] medicationIds)
        {
            //Arrange
            const string patientId = "123";
            MedicationStatus? epicMedicationStatus = MedicationStatus.Active;
            Models.MedicationStatus medicationStatus = Models.MedicationStatus.Active;
            var epicMedications = BuildEpicMedicationsWithCategories(patientId, epicMedicationStatus, epicMedicationCategories);
            ConfigureMapperWithMedicationCategories(epicMedications, medicationStatus, medicationCategories, epicMedicationCategories);
            IMedicationRepository repositoryMock = new MedicationRepository(
                _encounterContextMock.Object,
                _mapperMock.Object,
                _clientMock.Object,
                _patientIdClient.Object);

            //Act
            var results = (await repositoryMock.GetMedicationsAsync(patientId, medicationStatus, medicationCategories)).ToList();

            //Assert

            _mapperMock.Verify(
                x => x.Map<IEnumerable<MedicationCategory>>(medicationCategories),
                Times.Once());

            _clientMock.Verify(
                x => x.MedicationsAsync(patientId, It.IsAny<Guid>(), epicMedicationStatus, epicMedicationCategories, default),
                Times.Once());

            results.Should().HaveCount(epicMedications.Count);
            results.Select(x => x.Id).Intersect(medicationIds).Should().HaveCount(medicationIds.Length);
            results.Select(x => x.Id).Should().BeEquivalentTo(medicationIds);
        }

        private ICollection<Medication> BuildEpicMedicationsWithCategories(string patientId, MedicationStatus? epicMedicationStatus, MedicationCategory[] epicMedicationCategories)
        {
            ICollection<Medication> inpatient = new Medication[]
            {
                new ()
                {
                    Id = "inpatient"
                }
            };
            ICollection<Medication> outpatient = new Medication[]
            {
                new ()
                {
                    Id = "outpatient"
                }
            };
            ICollection<Medication> community = new Medication[]
            {
                new ()
                {
                    Id = "community"
                }
            };
            ICollection<Medication> specific = new Medication[]
            {
                new ()
                {
                    Id = "specific"
                }
            };
            ICollection<Medication> medications = Array.Empty<Medication>();
            MedicationCategory[] allCategories =
            {
                MedicationCategory.Community,
                MedicationCategory.Inpatient,
                MedicationCategory.Outpatient,
                MedicationCategory.PatientSpecified
            };
            foreach (var category in epicMedicationCategories ?? allCategories)
            {
                medications = category switch
                {
                    MedicationCategory.Community => medications.Concat(community).ToArray(),
                    MedicationCategory.Inpatient => medications.Concat(inpatient).ToArray(),
                    MedicationCategory.Outpatient => medications.Concat(outpatient).ToArray(),
                    MedicationCategory.PatientSpecified => medications.Concat(specific).ToArray(),
                    _ => medications
                };
            }

            _clientMock
                .Setup(client => client.MedicationsAsync(patientId, It.IsAny<Guid>(), epicMedicationStatus, epicMedicationCategories, default))
                .Returns(Task.FromResult(medications));

            return medications;
        }

        private void ConfigureMapperWithMedicationCategories(IEnumerable<Medication> epicMedications, Models.MedicationStatus? medicationStatus, Models.MedicationCategory[] medicationCategories, MedicationCategory[] medicationCategoriesMapped)
        {
            IEnumerable<Models.Medication> medications = epicMedications.Select(x => new Models.Medication { Id = x.Id });

            if (medicationStatus.HasValue)
            {
                _mapperMock
                    .Setup(mapper => mapper.Map<MedicationStatus>(medicationStatus.Value))
                    .Returns(medicationStatus.Value == Models.MedicationStatus.Active
                        ? MedicationStatus.Active
                        : MedicationStatus.Inactive);
            }

            if (medicationCategories == null)
            {
                _mapperMock
                    .Setup(mapper => mapper.Map<IEnumerable<MedicationCategory>>(medicationCategories))
                    .Returns<IEnumerable<MedicationCategory>>(null);
            }
            else if (medicationCategories.Any())
            {
                _mapperMock
                    .Setup(mapper => mapper.Map<IEnumerable<MedicationCategory>>(medicationCategories))
                    .Returns(medicationCategoriesMapped);
            }

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<Models.Medication>>(epicMedications))
                .Returns(medications);
        }
    }
}

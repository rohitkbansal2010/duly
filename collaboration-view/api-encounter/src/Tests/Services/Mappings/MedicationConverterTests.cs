// <copyright file="MedicationConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    internal class MedicationConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        private const string PrimaryCarePhysicianNamePrefixTemplate = "Dr.";

        [Test]
        public void ConvertTest_To_Regular()
        {
            //Arrange
            var source = BuildEmptyMedication();
            source.Dosages = BuildDosages(1);
            source.Period.Start = new DateTimeOffset(new DateTime(2021, 7, 15));
            source.Drug = new Drug
            {
                Title = "Test medicine."
            };
            source.Reason = new MedicationReason
            {
                ReasonText = new[] { "Test reason." }
            };
            source.Prescriber = new PractitionerGeneralInfo
            {
                Names = new[]
                {
                    new HumanName
                    {
                        FamilyName = "Reyes",
                        GivenNames = new[] { "Ana", "Maria" }
                    }
                },
                Roles = new[]
                {
                    new Role { Title = "Physician" }
                },
                Speciality = new List<string>() { "Licensed Counselor", "Behavioral Health" }
            };

            //Act
            var result = Mapper.Map<ApiContracts.Medication>(source);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be("test_id");
            result.ScheduleType.Should().Be(ApiContracts.MedicationScheduleType.Regular);
            result.Title.Should().Be("Test medicine.");
            result.Instructions.Should().Be("Test instruction.");
            result.Reason.Should().Be("Test reason.");
            result.StartDate.Should().Be(new DateTime(2021, 7, 15));
            result.Provider.Should().NotBeNull();
            result.Provider.HumanName.FamilyName.Should().Be("Reyes");
            result.Provider.HumanName.Prefixes.Should().NotBeNullOrEmpty();
            result.Provider.HumanName.Prefixes.Should().HaveCount(1);
            result.Provider.HumanName.Prefixes[0].Should().Be(PrimaryCarePhysicianNamePrefixTemplate);
        }

        [TestCase(0)]
        [TestCase(1, true)]
        [TestCase(2)]
        [TestCase(2, true)]
        public void ConvertTest_To_Other(int amount, bool asNeeded = false)
        {
            //Arrange
            var source = BuildEmptyMedication();
            source.Dosages = BuildDosages(amount, asNeeded);

            //Act
            var result = Mapper.Map<ApiContracts.Medication>(source);

            //Assert
            result.Should().NotBeNull();
            result.ScheduleType.Should().Be(ApiContracts.MedicationScheduleType.Other);
        }

        private static Medication BuildEmptyMedication()
        {
            return new Medication
            {
                Id = "test_id",
                Period = new Period(),
                Status = MedicationStatus.Active
            };
        }

        private static MedicationDosage[] BuildDosages(int amount, bool asNeeded = false)
        {
            var dosages = new List<MedicationDosage>();

            for (int i = 0; i < amount; i++)
            {
                dosages.Add(new MedicationDosage
                {
                    AsNeeded = asNeeded,
                    PatientInstruction = "Test instruction."
                });
            }

            return dosages.ToArray();
        }
    }
}

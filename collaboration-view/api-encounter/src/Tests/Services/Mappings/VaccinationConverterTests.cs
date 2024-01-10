// <copyright file="VaccinationConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using System;
using ApiContracts = Duly.CollaborationView.Encounter.Api.Contracts;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    internal class VaccinationConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        private const string Separator = @"\r\n";
        private const string AdministeredTitle = "ADMINISTERED";
        private const string NotAdministeredTitle = "NOT ADMINISTERED";

        [TestCase(PastImmunizationStatus.Completed)]
        [TestCase(PastImmunizationStatus.NotDone)]
        public void ConvertTest(PastImmunizationStatus status)
        {
            string expectedDateTitle;
            string expectedNotes;

            //Arrange
            var source = BuildFullPastImmunization(status);

            switch (status)
            {
                case PastImmunizationStatus.Completed:
                    expectedDateTitle = AdministeredTitle;
                    expectedNotes = $"{source.Notes[0]}{Separator}{source.Notes[1]}";
                    break;

                case PastImmunizationStatus.NotDone:
                    expectedDateTitle = NotAdministeredTitle;
                    expectedNotes = $"{source.StatusReason.Reason}{Separator}{source.Notes[0]}{Separator}{source.Notes[1]}";
                    break;

                case PastImmunizationStatus.EnteredInError:
                default:
                    expectedDateTitle = null;
                    expectedNotes = null;
                    break;
            }

            //Act
            var result = Mapper.Map<ApiContracts.Vaccination>(source);

            //Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(source.Vaccine.Text);
            result.Date.Should().Be(source.OccurrenceDateTime);
            result.DateTitle.Should().Be(expectedDateTitle);

            result.Dose.Should().NotBeNull();
            result.Dose.Amount.Should().Be(source.Dose.Value);
            result.Dose.Unit.Should().Be(source.Dose.Unit);

            result.Notes.Should().Be(expectedNotes);
        }

        [Test]
        public void ConvertTest_Without_Dose()
        {
            //Arrange
            var source = BuildFullPastImmunization(PastImmunizationStatus.Completed);
            source.Dose = null;

            //Act
            var result = Mapper.Map<ApiContracts.Vaccination>(source);

            //Assert
            result.Should().NotBeNull();
            result.Dose.Should().BeNull();
        }

        private static PastImmunization BuildFullPastImmunization(PastImmunizationStatus status)
        {
            return new PastImmunization
            {
                Id = "test_id",
                Status = status,
                OccurrenceDateTime = DateTimeOffset.UtcNow,
                Vaccine = new PastImmunizationVaccine
                {
                    Text = "TestVaccine"
                },
                Dose = new Quantity
                {
                    Value = 0.5M,
                    Unit = "mL"
                },
                Notes = new[] { "Test note 1.", "Test note 2." },
                StatusReason = new PastImmunizationStatusReason
                {
                    Reason = "Temporarily postponed."
                }
            };
        }
    }
}

// <copyright file="ImmunizationConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

using FhirModel = Hl7.Fhir.Model;
using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class ImmunizationConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertTest()
        {
            //Arrange
            var occurrenceDate = DateTimeOffset.Parse("2021-06-01");

            var source = new R4.Immunization
            {
                Id = Guid.NewGuid().ToString(),
                Status = R4.Immunization.ImmunizationStatusCodes.NotDone,
                DoseQuantity = new FhirModel.Quantity(),
                Note = new()
                {
                    new()
                    {
                        Text = new()
                        {
                            Value = "Annotation text"
                        }
                    }
                },
                VaccineCode = new FhirModel.CodeableConcept(),
                Occurrence = new FhirModel.FhirDateTime(occurrenceDate),
                StatusReason = new FhirModel.CodeableConcept
                {
                    Text = "Status reason text"
                }
            };

            //Act
            var result = Mapper.Map<Immunization>(source);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(source.Id);
            result.Dose.Should().NotBeNull();
            result.Status.Should().Be(ImmunizationStatus.NotDone);
            result.Vaccine.Should().NotBeNull();

            result.Notes.Should().NotBeNull();
            result.Notes.Should().HaveCount(source.Note.Count);
            result.Notes.Should().BeEquivalentTo(source.Note.Select(n => n.Text.Value));

            result.OccurrenceDateTime.Should().NotBeNull();
            result.OccurrenceDateTime.Should().Be(occurrenceDate);

            result.StatusReason.Should().NotBeNull();
            result.StatusReason.Reason.Should().Be(source.StatusReason.Text);
        }

        [Test]
        public void WhenNotesEmpty_ReturnsNullNotes_Test()
        {
            //Arrange
            var source = new R4.Immunization
            {
                Note = new()
            };

            //Act
            var result = Mapper.Map<Immunization>(source);

            //Assert
            result.Should().NotBeNull();
            result.Notes.Should().BeNull();
        }

        [Test]
        public void WhenStatusReasonNull_ReturnsNullStatusReason_Test()
        {
            //Arrange
            var source = new R4.Immunization();

            //Act
            var result = Mapper.Map<Immunization>(source);

            //Assert
            result.Should().NotBeNull();
            result.StatusReason.Should().BeNull();
        }

        [Test]
        public void WhenStatusReasonEmptyString_ReturnsNullStatusReason_Test()
        {
            //Arrange
            var source = new R4.Immunization
            {
                StatusReason = new()
                {
                    Text = string.Empty
                }
            };

            //Act
            var result = Mapper.Map<Immunization>(source);

            //Assert
            result.Should().NotBeNull();
            result.StatusReason.Should().BeNull();
        }
    }
}
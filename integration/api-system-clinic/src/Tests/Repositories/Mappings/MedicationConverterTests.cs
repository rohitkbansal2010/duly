// <copyright file="MedicationConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias stu3;

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using Duly.Clinic.Fhir.Adapter.Contracts;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using FhirModel = Hl7.Fhir.Model;

using STU3 = stu3::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class MedicationConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void Convert_Statement_Effective_IsNotPeriodOrDateTime_Test()
        {
            //Arrange
            var source = new MedicationStatementWithCompartments
            {
                Resource = new STU3.MedicationStatement
                {
                    Effective = new FhirModel.Code("cd"),
                    Medication = new FhirModel.ResourceReference("t", "sss")
                }
            };

            //Act
            Action action = () => Mapper.Map<Medication>(source);

            //Assert
            var exception = action.Should().Throw<ConceptNotMappedException>();
            exception.Which.Message.Should().Be("Could not map effective of medication statement");
        }

        [Test]
        public void Convert_Statement_PractitionerWithRoles_IsNull_Test()
        {
            //Arrange
            var source = new MedicationStatementWithCompartments
            {
                Resource = new STU3.MedicationStatement
                {
                    Effective = new FhirModel.Period(),
                    Medication = new FhirModel.ResourceReference("t", "sss")
                },
                Practitioner = null,
            };

            //Act
            var result = Mapper.Map<Medication>(source);

            //Assert
            result.Prescriber.Should().BeNull();
        }

        [Test]
        public void Convert_Statement_FindReason_IsNull_Test()
        {
            //Arrange
            var source = new MedicationStatementWithCompartments
            {
                Resource = new STU3.MedicationStatement
                {
                    Effective = new FhirModel.Period(),
                    Medication = new FhirModel.ResourceReference("t", "sss")
                },
                Practitioner = null
            };

            //Act
            var result = Mapper.Map<Medication>(source);

            //Assert
            result.Reason.Should().BeNull();
        }

        [Test]
        public void Convert_Statement_Effective_IsFhirDateTime_IsNull_Test()
        {
            //Arrange
            var source = new MedicationStatementWithCompartments
            {
                Resource = new STU3.MedicationStatement
                {
                    Effective = null,
                    Medication = new FhirModel.ResourceReference("t", "sss")
                }
            };

            //Act
            var result = Mapper.Map<Medication>(source);

            //Assert
            result.Prescriber.Should().BeNull();
        }

        [Test]
        public void ConvertTest()
        {
            //Arrange
            var start = new DateTimeOffset(2021, 12, 22, 0, 0, 0, 0, default);
            var end = new DateTimeOffset(2022, 12, 22, 0, 0, 0, 0, default);

            var source = new MedicationStatementWithCompartments
            {
                Resource = new STU3.MedicationStatement
                {
                    Effective = new FhirModel.Period
                    {
                        StartElement = new FhirModel.FhirDateTime(start),
                        EndElement = new FhirModel.FhirDateTime(end)
                    },
                    Id = "TestId",
                    Dosage = new List<STU3.Dosage>
                    {
                        new()
                        {
                            Dose = null
                        }
                    },
                    Medication = new FhirModel.ResourceReference("t", "sss")
                },
                Practitioner = new PractitionerWithRolesSTU3
                {
                    Resource = new STU3.Practitioner
                    {
                        Name = new List<STU3.HumanName>
                        {
                            STU3.HumanName.ForFamily("Test")
                        },
                    },
                    Roles = Array.Empty<STU3.PractitionerRole>()
                }
            };

            //Act
            var result = Mapper.Map<Medication>(source);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(source.Resource.Id);
            result.Period.Start.Should().Be(start);
            result.Period.End.Should().Be(end);
            result.Dosages.Should().NotBeEmpty();
            result.Prescriber.Should().NotBeNull();
            result.Status.Should().Be(MedicationStatus.Active);
        }

        [Test]
        public void Convert_FindReason_Test()
        {
            //Arrange
            var source = new MedicationStatementWithCompartments
            {
                Resource = new STU3.MedicationStatement
                {
                    Effective = null,
                    Medication = new FhirModel.ResourceReference("t", "sss"),
                    ReasonReference = new List<FhirModel.ResourceReference>
                    {
                        new("t", "asdasd")
                    }
                }
            };

            //Act
            var result = Mapper.Map<Medication>(source);

            //Assert
            result.Reason.ReasonText.First().Should().Be("asdasd");
        }
    }
}
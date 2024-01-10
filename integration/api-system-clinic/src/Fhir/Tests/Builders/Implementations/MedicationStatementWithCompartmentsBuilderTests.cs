// <copyright file="MedicationStatementWithCompartmentsBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias stu3;

using Duly.Clinic.Fhir.Adapter.Builders.Implementations;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using Duly.Clinic.Fhir.Adapter.Contracts;
using FluentAssertions;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System;
using System.Linq;

using STU3 = stu3::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Builders.Implementations
{
    [TestFixture]
    public class MedicationStatementWithCompartmentsBuilderTests
    {
        [Test]
        public void ExtractMedicationWithCompartmentsAsyncTest()
        {
            //Arrange
            var medicationId = Guid.NewGuid().ToString();
            var practitionerId = Guid.NewGuid().ToString();
            var observationId = Guid.NewGuid().ToString();

            var components = BuildEntryComponents(medicationId, practitionerId, observationId);
            var practitionersWithRoles = BuildPractitionerWithRolesStu3s(practitionerId);

            IMedicationStatementWithCompartmentsBuilder builder = new MedicationStatementWithCompartmentsBuilder();

            //Act
            var result = builder.ExtractMedicationWithCompartments(components, practitionersWithRoles);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Resource.Should().NotBeNull();
            result.First().Resource.Id.Should().Be(medicationId);
            result.First().Practitioner.Resource.Should().NotBeNull();
            result.First().Practitioner.Resource.Id.Should().Be(practitionerId);
        }

        private static PractitionerWithRolesSTU3[] BuildPractitionerWithRolesStu3s(string practitionerId)
        {
            var practitionersWithRoles = new PractitionerWithRolesSTU3[]
            {
                new()
                {
                    Resource = new() { Id = practitionerId },
                    Roles = new[]
                    {
                        new STU3.PractitionerRole { Id = Guid.NewGuid().ToString() },
                    }
                }
            };
            return practitionersWithRoles;
        }

        private static STU3.Bundle.EntryComponent[] BuildEntryComponents(string medicationId, string practitionerId, string observationId)
        {
            var components = new STU3.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new STU3.MedicationStatement
                    {
                        Id = medicationId,
                        InformationSource = new ResourceReference(nameof(STU3.Practitioner) + "/" + practitionerId),
                        ReasonReference = new()
                        {
                            new(nameof(STU3.Observation) + "/" + observationId)
                        }
                    }
                },
                new()
                {
                    Resource = new STU3.Observation { Id = observationId }
                }
            };
            return components;
        }
    }
}

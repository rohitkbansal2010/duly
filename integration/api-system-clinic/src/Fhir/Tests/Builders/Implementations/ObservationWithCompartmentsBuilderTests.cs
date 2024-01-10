// <copyright file="ObservationWithCompartmentsBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Builders.Implementations;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Builders.Implementations
{
    [TestFixture]
    public class ObservationWithCompartmentsBuilderTests
    {
        [Test]
        public void ExtractOrganizationsTest()
        {
            //Arrange
            var observationId = Guid.NewGuid().ToString();
            var components = new R4.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new R4.Observation { Id = observationId }
                }
            };

            IObservationWithCompartmentsBuilder builder = new ObservationWithCompartmentsBuilder();

            //Act
            var result = builder.ExtractObservations(components);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Resource.Id.Should().Be(observationId);
        }
    }
}

// <copyright file="RelatedPersonWithCompartmentsBuilderTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
extern alias r4;

using Duly.Clinic.Fhir.Adapter.Builders.Implementations;
using Duly.Clinic.Fhir.Adapter.Builders.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Fhir.Adapter.Tests.Builders.Implementations
{
    [TestFixture]
    public class RelatedPersonWithCompartmentsBuilderTests
    {
        [Test]
        public void ExtractRelatedPersonsTest()
        {
            //Arrange
            var relatedPersonId = "relatedPersonId";
            var components = new R4.Bundle.EntryComponent[]
            {
                new()
                {
                    Resource = new R4.RelatedPerson { Id = relatedPersonId }
                }
            };

            IRelatedPersonWithCompartmentsBuilder builder = new RelatedPersonWithCompartmentsBuilder();

            //Act
            var result = builder.ExtractRelatedPersons(components);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result.First().Resource.Id.Should().Be(relatedPersonId);
        }
    }
}

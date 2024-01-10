// <copyright file="AllergyIntoleranceConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

extern alias r4;

using Duly.Clinic.Api.Repositories.Mappings;
using Duly.Clinic.Api.Tests.Common;
using Duly.Clinic.Contracts;
using FluentAssertions;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using R4 = r4::Hl7.Fhir.Model;

namespace Duly.Clinic.Api.Tests.Repositories.Mappings
{
    [TestFixture]
    public class AllergyIntoleranceConverterTests : MapperConfigurator<ClientToSystemApiContractsProfile>
    {
        [Test]
        public void ConvertTest()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var recordedDate = new DateTimeOffset(DateTime.UtcNow);

            var source = new R4.AllergyIntolerance
            {
                Id = id,
                RecordedDateElement = new FhirDateTime(recordedDate),
                Code = new CodeableConcept("system", "code", "Allergen name"),
                Category = new R4.AllergyIntolerance.AllergyIntoleranceCategory?[]
                {
                    R4.AllergyIntolerance.AllergyIntoleranceCategory.Medication,
                    R4.AllergyIntolerance.AllergyIntoleranceCategory.Food
                },
                Reaction = new List<R4.AllergyIntolerance.ReactionComponent>
                {
                    new R4.AllergyIntolerance.ReactionComponent
                    {
                        Description = "Reaction_1",
                        Severity = R4.AllergyIntolerance.AllergyIntoleranceSeverity.Mild
                    },
                    new R4.AllergyIntolerance.ReactionComponent
                    {
                        Description = "Reaction_2",
                        Severity = R4.AllergyIntolerance.AllergyIntoleranceSeverity.Severe
                    }
                }
            };

            //Act
            var result = Mapper.Map<AllergyIntolerance>(source);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.RecordedDate.Should().Be(recordedDate);
            result.AllergenName.Should().Be("Allergen name");

            result.Categories.Should().NotBeNullOrEmpty();
            result.Categories.Should().HaveCount(source.CategoryElement.Count);
            result.Categories.Last().Should().Be(AllergyIntoleranceCategory.Food);

            result.Reactions.Should().NotBeNullOrEmpty();
            result.Reactions.Should().HaveCount(source.Reaction.Count);
            var lastReaction = result.Reactions.Last();
            lastReaction.Title.Should().Be("Reaction_2");
            lastReaction.Severity.Should().Be(AllergyIntoleranceReactionSeverity.Severe);
        }
    }
}
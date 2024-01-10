// <copyright file="AllergyConverterTests.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using Duly.CollaborationView.Encounter.Api.Repositories.Models;
using Duly.CollaborationView.Encounter.Api.Services.Mappings;
using Duly.CollaborationView.Encounter.Api.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Tests.Services.Mappings
{
    [TestFixture]
    internal class AllergyConverterTests : MapperConfigurator<RepositoryModelsToProcessContractsProfile>
    {
        [Test]
        public void ConvertTest()
        {
            //Arrange
            IEnumerable<AllergyIntolerance> source = new List<AllergyIntolerance>
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    RecordedDate = new DateTimeOffset(2017, 01, 21, 0, 0, 0, default),
                    AllergenName = "Latex",
                    Categories = new[]
                    {
                        AllergyIntoleranceCategory.Food,
                        AllergyIntoleranceCategory.Medication,
                        AllergyIntoleranceCategory.Biologic,
                        AllergyIntoleranceCategory.Environment,
                        AllergyIntoleranceCategory.Other
                    },
                    Reactions = new AllergyIntoleranceReaction[]
                    {
                        new() { Title = "Rash", Severity = AllergyIntoleranceReactionSeverity.Severe },
                        new() { Title = "Whatever", Severity = AllergyIntoleranceReactionSeverity.Mild },
                        new() { Title = "Something", Severity = AllergyIntoleranceReactionSeverity.Mild },
                        new() { Title = "Anything", Severity = AllergyIntoleranceReactionSeverity.Moderate },
                    }
                },
                new()
                {
                    Id = "testId",
                    RecordedDate = new DateTimeOffset(2010, 01, 21, 0, 0, 0, default),
                    AllergenName = "Gluten",
                    Categories = new[]
                    {
                        AllergyIntoleranceCategory.Food,
                    },
                    Reactions = new AllergyIntoleranceReaction[]
                    {
                        new() { Title = "Red skin", Severity = AllergyIntoleranceReactionSeverity.Mild },
                    }
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    RecordedDate = new DateTimeOffset(2020, 01, 21, 0, 0, 0, default),
                    Categories = Array.Empty<AllergyIntoleranceCategory>(),
                    Reactions = Array.Empty<AllergyIntoleranceReaction>()
                }
            };

            //Act
            var result = Mapper.Map<IEnumerable<Allergy>>(source);

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(3);
            result.First().Should().NotBeNull();
            result.First().Categories.First().Should().Be(AllergyCategory.Biologic);
            result.First().Categories[4].Should().Be(AllergyCategory.Other);
            result.First().Reactions.First().Title.Should().Be("Anything");
            result.First().Reactions.First().Severity.Should().Be(AllergyReactionSeverity.Moderate);
            result.First().Title.Should().Be("Latex");
        }
    }
}

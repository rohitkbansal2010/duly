// -----------------------------------------------------------------------
// <copyright file="AllergyIntolerance.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class AllergyIntoleranceExampleProvider : IExamplesProvider<IEnumerable<AllergyIntolerance>>
    {
        public IEnumerable<AllergyIntolerance> GetExamples()
        {
            yield return new AllergyIntolerance
            {
                AllergenName = "Peanuts",
                RecordedDate = new DateTimeOffset(2020, 11, 2, 0, 0, 0, default),
                Categories = new[]
                {
                    AllergyIntoleranceCategory.Food
                },
                Reactions = new AllergyIntoleranceReaction[]
                {
                    new()
                    {
                        Title = "Anaphylaxis",
                        Severity = AllergyIntoleranceReactionSeverity.Severe
                    }
                }
            };
            yield return new AllergyIntolerance
            {
                AllergenName = "Latex",
                RecordedDate = new DateTimeOffset(2015, 7, 5, 0, 0, 0, default),
                Categories = new[]
                {
                    AllergyIntoleranceCategory.Biologic,
                    AllergyIntoleranceCategory.Food
                },
                Reactions = new AllergyIntoleranceReaction[]
                {
                    new()
                    {
                        Title = "Rash",
                        Severity = AllergyIntoleranceReactionSeverity.Mild
                    }
                }
            };
            yield return new AllergyIntolerance
            {
                AllergenName = "Orange",
                RecordedDate = new DateTimeOffset(2021, 11, 21, 0, 0, 0, default),
                Categories = new[]
                {
                    AllergyIntoleranceCategory.Food
                },
                Reactions = new AllergyIntoleranceReaction[]
                {
                    new()
                    {
                        Title = "Hippopotomonstrosesquippedaliophobia and now it wrapped",
                        Severity = AllergyIntoleranceReactionSeverity.Severe
                    },
                    new()
                    {
                        Title = "Rash",
                        Severity = AllergyIntoleranceReactionSeverity.Mild
                    }
                }
            };
        }
    }
}
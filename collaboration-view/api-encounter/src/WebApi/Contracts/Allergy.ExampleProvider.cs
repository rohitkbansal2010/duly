// -----------------------------------------------------------------------
// <copyright file="Allergy.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Examples")]
    public class AllergyExampleProvider : IExamplesProvider<Allergy>
    {
        public Allergy GetExamples()
        {
            return BuildAllergy(DateTime.Today, new[] { AllergyCategory.Medication }, true);
        }

        public Allergy BuildAllergy(
            DateTime date,
            AllergyCategory[] categories,
            bool isMultipleReactions)
        {
            var allergy = new Allergy
            {
                Id = Guid.NewGuid().ToString("N"),
                Recorded = date,
                Categories = categories
            };

            CheckCategory(categories, isMultipleReactions, allergy);

            return allergy;
        }

        private static void CheckCategory(AllergyCategory[] categories, bool isMultipleReactions, Allergy allergy)
        {
            switch (categories?.Length)
            {
                case > 1:
                    HandleOther(isMultipleReactions, allergy);
                    break;
                case 1:
                    HandleSingleCategory(categories, isMultipleReactions, allergy);
                    break;
            }
        }

        private static void HandleSingleCategory(AllergyCategory[] categories, bool isMultipleReactions, Allergy allergy)
        {
            allergy.Title = "Latex";
            switch (categories[0])
            {
                case AllergyCategory.Food:
                    HandleFood(isMultipleReactions, allergy);
                    break;

                case AllergyCategory.Medication:
                    HandleMedication(isMultipleReactions, allergy);
                    break;

                case AllergyCategory.Environment:
                    HandleEnvironment(isMultipleReactions, allergy);
                    break;

                case AllergyCategory.Biologic:
                    HandleEnvironment(isMultipleReactions, allergy);
                    break;
                default:
                    HandleOther(isMultipleReactions, allergy);
                    break;
            }
        }

        private static void HandleOther(bool isMultipleReactions, Allergy allergy)
        {
            allergy.Title = "Latex";
            if (isMultipleReactions)
            {
                allergy.Reactions = new[]
                {
                    new AllergyReaction
                    {
                        Title = "Rush",
                        Severity = AllergyReactionSeverity.Mild
                    },
                    new AllergyReaction
                    {
                        Title = "Skin ulcer",
                        Severity = AllergyReactionSeverity.Severe
                    }
                };
            }
            else
            {
                allergy.Reactions = new[]
                {
                    new AllergyReaction
                    {
                        Title = "Rush",
                        Severity = AllergyReactionSeverity.Mild
                    }
                };
            }
        }

        private static void HandleEnvironment(bool isMultipleReactions, Allergy allergy)
        {
            allergy.Title = "Pollen";
            if (isMultipleReactions)
            {
                allergy.Reactions = new[]
                {
                    new AllergyReaction
                    {
                        Title = "Congestion",
                        Severity = AllergyReactionSeverity.Mild
                    },
                    new AllergyReaction
                    {
                        Title = "Rush",
                        Severity = AllergyReactionSeverity.Mild
                    }
                };
            }
            else
            {
                allergy.Reactions = new[]
                {
                    new AllergyReaction
                    {
                        Title = "Congestion",
                        Severity = AllergyReactionSeverity.Mild
                    }
                };
            }
        }

        private static void HandleMedication(bool isMultipleReactions, Allergy allergy)
        {
            allergy.Title = "Penicillin";
            if (isMultipleReactions)
            {
                allergy.Reactions = new[]
                {
                    new AllergyReaction
                    {
                        Title = "Itching",
                        Severity = AllergyReactionSeverity.Moderate
                    },
                    new AllergyReaction
                    {
                        Title = "Rush",
                        Severity = AllergyReactionSeverity.Mild
                    }
                };
            }
            else
            {
                allergy.Reactions = new[]
                {
                    new AllergyReaction
                    {
                        Title = "Itching",
                        Severity = AllergyReactionSeverity.Moderate
                    }
                };
            }
        }

        private static void HandleFood(bool isMultipleReactions, Allergy allergy)
        {
            allergy.Title = "Peanuts";
            if (isMultipleReactions)
            {
                allergy.Reactions = new[]
                {
                    new AllergyReaction
                    {
                        Title = "Anaphylaxis",
                        Severity = AllergyReactionSeverity.Severe
                    },
                    new AllergyReaction
                    {
                        Title = "Rush",
                        Severity = AllergyReactionSeverity.Mild
                    }
                };
            }
            else
            {
                allergy.Reactions = new[]
                {
                    new AllergyReaction
                    {
                        Title = "Anaphylaxis",
                        Severity = AllergyReactionSeverity.Severe
                    }
                };
            }
        }
    }
}
// -----------------------------------------------------------------------
// <copyright file="HealthConditions.ExampleProvider.cs" company="Duly Health and Care">
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
    public class HealthConditionsExampleProvider : IExamplesProvider<HealthConditions>
    {
        public HealthConditions GetExamples()
        {
            var provider = new HealthConditionExampleProvider();
            DateTime baseDate = DateTime.Today;

            return new HealthConditions
            {
                PreviousHealthConditions = new[]
                {
                    provider.BuildExampleFor(
                            "Pneumonia",
                            new DateTimeOffset(baseDate.AddDays(-10))),

                    provider.BuildExampleFor(
                        "Dehydration",
                        new DateTimeOffset(baseDate.AddDays(-20))),

                    provider.BuildExampleFor(
                        "Myocardial infarction (STEMI)",
                        new DateTimeOffset(baseDate.AddDays(-60))),

                    provider.BuildExampleFor(
                        "Hypertension associated with diabetes (HCC)"),

                    provider.BuildExampleFor(
                        "Kidney stones")
                },
                CurrentHealthConditions = new[]
                {
                    provider.BuildExampleFor(
                        "Pneumonia",
                        new DateTimeOffset(baseDate.AddDays(-5))),

                    provider.BuildExampleFor(
                        "Chronic sinusitis",
                        new DateTimeOffset(baseDate.AddDays(-1000))),

                    provider.BuildExampleFor(
                        "Diabetes mellitus, type II"),

                    provider.BuildExampleFor(
                        "Osteoarthritis")
                }
            };
        }
   }
}
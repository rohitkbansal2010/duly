// -----------------------------------------------------------------------
// <copyright file="HealthCondition.ExampleProvider.cs" company="Duly Health and Care">
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
    public class HealthConditionExampleProvider : IExamplesProvider<HealthCondition>
    {
        public HealthCondition GetExamples()
        {
            return BuildExampleFor("Diabetes mellitus, type II", new DateTimeOffset(DateTime.Today));
        }

        public HealthCondition BuildExampleFor(string title, DateTimeOffset? date = null)
        {
            return new HealthCondition
            {
                Date = date,
                Title = string.IsNullOrWhiteSpace(title) ? "Pneumonia" : title
            };
        }
    }
}
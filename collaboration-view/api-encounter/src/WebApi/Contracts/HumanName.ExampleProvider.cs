// -----------------------------------------------------------------------
// <copyright file="HumanName.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class HumanNameExampleProvider : IExamplesProvider<HumanName>
    {
        public HumanName GetExamples()
        {
            return new HumanName
            {
                FamilyName = "Reyes",
                GivenNames = new[] { "Ana", "Maria" },
                Prefixes = new[] { "Dr." }
            };
        }
    }
}
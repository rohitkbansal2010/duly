// -----------------------------------------------------------------------
// <copyright file="HumanName.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class HumanNameExampleProvider : IExamplesProvider<HumanName>
    {
        public static HumanName[] GetNames(params HumanName[] names)
        {
            return names;
        }

        public static HumanName Build(string family, string given = null, string prefix = null)
        {
            return new HumanName
            {
                FamilyName = family,
                GivenNames = new[] { given },
                Prefixes = new[] { prefix }
            };
        }

        public HumanName GetExamples()
        {
            return new HumanName
            {
                FamilyName = "Reyes",
                GivenNames = new[] { "Ana", "Maria" },
                Prefixes = new[] { "Mrs." }
            };
        }

        public HumanName GetPractitionerHumanNameExample()
        {
            return new HumanName
            {
                FamilyName = "Sussman",
                GivenNames = new[] { "Miley" }
            };
        }

        public HumanName GetPersonHumanNameExample()
        {
            return new HumanName
            {
                FamilyName = "Pruitt",
                GivenNames = new[] { "Warren", "Nikki" }
            };
        }
    }
}
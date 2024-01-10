// -----------------------------------------------------------------------
// <copyright file="Allergy.ExamplesProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Examples")]
    public class AllergyExamplesProvider : IExamplesProvider<IEnumerable<Allergy>>
    {
        public IEnumerable<Allergy> GetExamples()
        {
            var provider = new AllergyExampleProvider();

            yield return provider.BuildAllergy(DateTime.Today, new[] { AllergyCategory.Food }, false);
            yield return provider.BuildAllergy(DateTime.Today.AddDays(-3), new[] { AllergyCategory.Medication }, true);
            yield return provider.BuildAllergy(DateTime.Today.AddDays(-7), new[] { AllergyCategory.Biologic }, false);
            yield return provider.BuildAllergy(DateTime.Today.AddDays(-11), new[] { AllergyCategory.Environment }, true);
            yield return provider.BuildAllergy(DateTime.Today.AddDays(-17), new[] { AllergyCategory.Biologic, AllergyCategory.Food }, false);
        }
    }
}
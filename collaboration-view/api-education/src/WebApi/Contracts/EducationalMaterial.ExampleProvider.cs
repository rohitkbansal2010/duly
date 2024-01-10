// -----------------------------------------------------------------------
// <copyright file="EducationalMaterial.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Duly.Education.Api.Contracts
{
#pragma warning disable SA1649 // File name should match first type name

    public class EducationalMaterialExampleProvider : IExamplesProvider<IEnumerable<EducationalMaterial>>
    {
        public IEnumerable<EducationalMaterial> GetExamples()
        {
            yield return new EducationalMaterial
            {
            };
            yield return new EducationalMaterial
            {
            };
            yield return new EducationalMaterial
            {
            };
        }
    }

#pragma warning restore SA1649 // File name should match first type name
}
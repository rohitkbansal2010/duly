// -----------------------------------------------------------------------
// <copyright file="Practitioners.GeneralInfo.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class PractitionersGeneralInfoExampleProvider : IExamplesProvider<IEnumerable<PractitionerGeneralInfo>>
    {
        public IEnumerable<PractitionerGeneralInfo> GetExamples()
        {
            var provider = new PractitionerGeneralInfoExampleProvider();
            yield return provider.GetExamples();
        }
    }
}
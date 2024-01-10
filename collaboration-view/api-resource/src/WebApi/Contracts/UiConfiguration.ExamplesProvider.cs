// -----------------------------------------------------------------------
// <copyright file="UiConfiguration.ExamplesProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Duly.CollaborationView.Resource.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class UiConfigurationExamplesProvider : IExamplesProvider<IEnumerable<UiConfiguration>>
    {
        public IEnumerable<UiConfiguration> GetExamples()
        {
            yield return new NavigationModulesUiConfigurationExampleProvider().GetExamples();
        }
    }
}
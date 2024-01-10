// <copyright file="RelatedPerson.GeneralInfo.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class RelatedPersonGeneralInfoExampleProvider : IExamplesProvider<RelatedPersonGeneralInfo>
    {
        public RelatedPersonGeneralInfo GetExamples()
        {
            var hnProvider = new HumanNameExampleProvider();
            return new RelatedPersonGeneralInfo
            {
                Id = Guid.NewGuid().ToString(),
                Names = HumanNameExampleProvider.GetNames(hnProvider.GetPersonHumanNameExample()),
                Photos = new[] { PhotoExample.MakeAttachment() }
            };
        }
    }
}
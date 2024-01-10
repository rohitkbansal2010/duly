// -----------------------------------------------------------------------
// <copyright file="Practitioner.GeneralInfo.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class PractitionerGeneralInfoExampleProvider : IExamplesProvider<PractitionerGeneralInfo>
    {
        public PractitionerGeneralInfo GetExamples()
        {
            var provider = new HumanNameExampleProvider();
            return new PractitionerGeneralInfo
            {
                Id = "qwerty1",
                Names = HumanNameExampleProvider.GetNames(provider.GetPractitionerHumanNameExample()),
                Roles = new[]
                {
                    new Role { Title = "Physician" }
                },
                Photos = new[] { PhotoExample.MakeAttachment() }
            };
        }
    }
}
// -----------------------------------------------------------------------
// <copyright file="Practitioner.GeneralInfo.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.CollaborationView.Encounter.Api.Contracts.Extensions;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class PractitionerGeneralInfoExampleProvider : IExamplesProvider<PractitionerGeneralInfo>
    {
        public PractitionerGeneralInfo GetExamples()
        {
            return new PractitionerGeneralInfo
            {
                Id = "qwerty1",
                HumanName = new HumanName
                {
                    FamilyName = "Reyes",
                    GivenNames = new[] { "Ana", "Maria" },
                    Prefixes = new[] { "Dr." }
                },
                Role = PractitionerRole.PrimaryCarePhysician,
                Photo = PhotoExample.MakeAttachment()
            };
        }
    }

    public class PractitionersGeneralInfoExampleProvider : IExamplesProvider<IEnumerable<PractitionerGeneralInfo>>
    {
        public IEnumerable<PractitionerGeneralInfo> GetExamples()
        {
            yield return new PractitionerGeneralInfo
            {
                Id = "qwerty0",
                HumanName = new HumanName
                {
                    FamilyName = "Goodspeed",
                    GivenNames = new[] { "Jane" }
                },
                Role = PractitionerRole.MedicalAssistant,
            };
            yield return new PractitionerGeneralInfoExampleProvider().GetExamples();
            yield return new PractitionerGeneralInfo
            {
                Id = "qwerty2",
                HumanName = new HumanName
                {
                    FamilyName = "Ling",
                    GivenNames = new[] { "Marty" }
                },
                Role = PractitionerRole.PrimaryCarePhysician,
            };
        }
    }
}
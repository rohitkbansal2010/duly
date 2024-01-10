// -----------------------------------------------------------------------
// <copyright file="Patient.GeneralInfo.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class PatientGeneralInfoExampleProvider : IExamplesProvider<PatientGeneralInfo>
    {
        public PatientGeneralInfo GetExamples()
        {
            return new PatientGeneralInfo
            {
                Id = "qwerty1",
                HumanName = new HumanName
                {
                    FamilyName = "Reyes",
                    GivenNames = new[] { "Ana", "Maria" }
                }
            };
        }
    }
}
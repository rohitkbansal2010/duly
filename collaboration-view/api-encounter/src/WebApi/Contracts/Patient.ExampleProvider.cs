// <copyright file="Patient.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts.Extensions;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class PatientExampleProvider : IExamplesProvider<Patient>
    {
        public Patient GetExamples()
        {
            var provider = new PatientGeneralInfoExampleProvider();
            return new Patient
            {
                GeneralInfo = provider.GetExamples(),
                BirthDate = new DateTime(1955, 12, 21),
                Gender = Gender.Female,

                //Photo = PhotoExample.MakeAttachment()
            };
        }
    }
}
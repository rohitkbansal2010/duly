// <copyright file="Patient.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Duly.Clinic.Api.ExampleProviders
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class PatientExampleProvider : IExamplesProvider<Patient>
    {
        public Patient GetExamples()
        {
            var hnProvider = new HumanNameExampleProvider();
            return new Patient
            {
                PatientGeneralInfo = new PatientGeneralInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    Names = HumanNameExampleProvider.GetNames(hnProvider.GetPersonHumanNameExample())
                },
                Gender = Gender.Female,
                BirthDate = new DateTime(1955, 12, 20),
            };
        }
    }
}

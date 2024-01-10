// -----------------------------------------------------------------------
// <copyright file="Medications.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Examples")]
    public class MedicationsExampleProvider : IExamplesProvider<Medications>
    {
        public Medications GetExamples()
        {
            return new Medications
            {
                Regular = new[]
                {
                    new MedicationExampleProvider().BuildExampleFor(MedicationScheduleType.Regular)
                },
                Other = new[]
                {
                    new MedicationExampleProvider().BuildExampleFor(MedicationScheduleType.Other)
                }
            };
        }
    }
}
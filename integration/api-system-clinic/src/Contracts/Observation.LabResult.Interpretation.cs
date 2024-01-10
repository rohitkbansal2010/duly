// <copyright file="Observation.LabResult.Interpretation.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Categorical assessment of the value of the laboratory observation result.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    public class ObservationLabResultInterpretation
    {
        [Description("Code according to the system: http://hl7.org/fhir/ValueSet/observation-interpretation.")]
        public string Code { get; set; }

        [Description("Description of the categorical assessment of the value of the laboratory observation result.")]
        public string Text { get; set; }
    }
}

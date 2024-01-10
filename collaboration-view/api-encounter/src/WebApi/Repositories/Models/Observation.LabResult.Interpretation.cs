// <copyright file="Observation.LabResult.Interpretation.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Categorical assessment of the value of the laboratory observation result.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    internal class ObservationLabResultInterpretation
    {
        /// <summary>
        /// Code according to the system: http://hl7.org/fhir/ValueSet/observation-interpretation.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Description of the categorical assessment of the value of the laboratory observation result.
        /// </summary>
        public string Text { get; set; }
    }
}

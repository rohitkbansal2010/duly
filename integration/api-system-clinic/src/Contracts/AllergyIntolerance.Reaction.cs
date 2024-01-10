// -----------------------------------------------------------------------
// <copyright file="AllergyIntolerance.Reaction.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("An allergic reaction in a patient.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    public class AllergyIntoleranceReaction
    {
        [Description("An allergy intolerance reaction name.")]
        [Required]
        public string Title { get; set; }

        [Description("The degree of the patient's reaction.")]
        [Required]
        public AllergyIntoleranceReactionSeverity Severity { get; set; }
    }
}

// -----------------------------------------------------------------------
// <copyright file="Immunizations.Progress.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents information on the progress of a patient's immunization.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    public class ImmunizationsProgress
    {
        internal const int LaggingBehindLT = 70;
        internal const int AlmostLT = 100;

        [Description("Percentage of the patient's immunizations completed.")]
        [Required]
        [Range(0, 100)]
        public int PercentageCompletion { get; set; }

        [Description("The number of immunizations groups recommended to the patient.")]
        [Required]
        public int RecommendedGroupNumber { get; set; }

        [Description("Immunizations completion status.")]
        [Required]
        public ImmunizationsCompletionStatus CompletionStatus
        {
            get
            {
                return PercentageCompletion switch
                {
                    < LaggingBehindLT => ImmunizationsCompletionStatus.LaggingBehind,
                    >= LaggingBehindLT and < AlmostLT => ImmunizationsCompletionStatus.Almost,
                    _ => ImmunizationsCompletionStatus.Completed
                };
            }
        }
    }
}

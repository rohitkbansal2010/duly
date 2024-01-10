// -----------------------------------------------------------------------
// <copyright file="AllergyIntolerance.Reaction.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    internal class AllergyIntoleranceReaction
    {
        /// <summary>
        /// An allergy intolerance reaction name.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The degree of the patient's reaction.
        /// </summary>
        public AllergyIntoleranceReactionSeverity Severity { get; set; }
    }
}

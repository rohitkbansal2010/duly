// <copyright file="AllergyIntolerance.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Information about the allergy intolerance.
    /// </summary>
    internal class AllergyIntolerance
    {
        /// <summary>
        /// Allergy Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Allergen name.
        /// </summary>
        public string AllergenName { get; set; }

        /// <summary>
        /// The date the allergy intolerance was recorded in data source.
        /// </summary>
        public DateTimeOffset RecordedDate { get; set; }

        /// <summary>
        /// All categories of an identified substance associated with allergy intolerances.
        /// </summary>
        public AllergyIntoleranceCategory[] Categories { get; set; }

        /// <summary>
        /// Allergy intolerance reactions.
        /// </summary>
        public AllergyIntoleranceReaction[] Reactions { get; set; }
    }
}

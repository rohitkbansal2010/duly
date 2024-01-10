// -----------------------------------------------------------------------
// <copyright file="Allergy.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Information on the identified allergy-related substance and the patient's reactions to that substance.")]
    public class Allergy
    {
        [Description("Allergy Id.")]
        [Required]
        public string Id { get; set; }

        [Description("Name of an identified substance associated with allergies.")]
        public string Title { get; set; }

        [Description("The date the allergy was recorded in Epic.")]
        [Required]
        public DateTimeOffset Recorded { get; set; }

        [Description("All categories of an identified substance associated with allergies.")]
        [Required]
        [MinLength(1)]
        public AllergyCategory[] Categories { get; set; }

        [Description("Allergy reactions.")]
        [Required]
        public AllergyReaction[] Reactions { get; set; }
    }
}

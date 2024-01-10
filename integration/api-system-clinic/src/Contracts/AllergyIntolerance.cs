// -----------------------------------------------------------------------
// <copyright file="AllergyIntolerance.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Contracts.Interfaces;
using Duly.Common.Security.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("The data about an allergy or intolerance to a specific substance associated with one patient.")]
    public class AllergyIntolerance : IDulyResource
    {
        [Description("Allergy Id")]
        [Required]
        [Identity]
        public string Id { get; set; }

        [Description("Allergen name")]
        public string AllergenName { get; set; }

        [Description("The date the allergy intolerance was recorded in data source")]
        [Required]
        public DateTimeOffset RecordedDate { get; set; }

        [Description("All categories of an identified substance associated with allergy intolerances.")]
        [Required]
        [MinLength(1)]
        public AllergyIntoleranceCategory[] Categories { get; set; }

        [Description("Allergy intolerance reactions")]
        [Required]
        public AllergyIntoleranceReaction[] Reactions { get; set; }
    }
}
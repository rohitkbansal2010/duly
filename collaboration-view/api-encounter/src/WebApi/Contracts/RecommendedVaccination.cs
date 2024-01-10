// -----------------------------------------------------------------------
// <copyright file="RecommendedVaccination.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about a recommended vaccination.")]
    public class RecommendedVaccination
    {
        [Description("The title of the vaccine.")]
        public string Title { get; set; }

        [Description("Vaccination status.")]
        [Required]
        public RecommendedVaccinationStatus Status { get; set; }

        [Description("The title of the date of the corresponding vaccination status.")]
        [Required]
        public string DateTitle { get; set; }

        [Description("Date of the respective vaccination status.")]
        public DateTimeOffset? Date { get; set; }

        [Description("Vaccination notes.")]
        public string Notes { get; set; }
    }
}

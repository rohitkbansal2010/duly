// -----------------------------------------------------------------------
// <copyright file="Vaccination.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about a vaccination.")]
    public class Vaccination
    {
        [Description("The title of the vaccine.")]
        [Required]
        public string Title { get; set; }

        [Description("The title of the date the patient was immunized or not: '" + Immunizations.AdministeredTitle + "' or '" + Immunizations.NotAdministeredTitle + "'.")]
        [Required]
        public string DateTitle { get; set; }

        [Description("The date the patient was immunized or not.")]
        public DateTimeOffset? Date { get; set; }

        [Description("The dose of vaccine administered.")]
        public Dose Dose { get; set; }

        [Description("Vaccination notes.")]
        public string Notes { get; set; }
    }
}

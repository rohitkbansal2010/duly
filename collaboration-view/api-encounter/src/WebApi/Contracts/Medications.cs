// -----------------------------------------------------------------------
// <copyright file="Medications.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Information about medications that the patient is/was taking, divided by the type of schedule.")]
    public class Medications
    {
        [Description("Information about regular medications that the patient is/was taking.")]
        [Required]
        public Medication[] Regular { get; set; }

        [Description("Information about other medications that the patient is/was taking.")]
        [Required]
        public Medication[] Other { get; set; }
    }
}
// -----------------------------------------------------------------------
// <copyright file="Medication.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Represents an information about the medication the patient is/was taking.")]
    public class Medication
    {
        [Description("An identifier for the medication")]
        [Required]
        public string Id { get; set; }

        [Description("Medication schedule type")]
        [Required]
        public MedicationScheduleType ScheduleType { get; set; }

        [Description("The name of the medicine, including the brand name, active ingredient and dosage.")]
        public string Title { get; set; }

        [Description("A reason why the medication the patient is/was taking.")]
        public string Reason { get; set; }

        [Description("The date the patient started taking the medicine.")]
        public DateTimeOffset? StartDate { get; set; }

        [Description("The person that provided the information about the taking of this medication.")]
        public PractitionerGeneralInfo Provider { get; set; }

        [Description("The patient instructions for the prescription.")]
        public string Instructions { get; set; }
    }
}
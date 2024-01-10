// <copyright file="Medication.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Common.Security.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Record of medication being taken by a patient")]
    public class Medication
    {
        [Description("Business identifier for this medication")]
        [Required]
        [Identity]
        public string Id { get; set; }

        [Description("Drug information")]
        public Drug Drug { get; set; }

        [Description("Medication status")]
        public MedicationStatus Status { get; set; }

        [Description("Reason for why the medication is being/was taken")]
        public MedicationReason Reason { get; set; }

        [Description("The interval when the medication is/was/will be taken")]
        public Period Period { get; set; }

        [Description("Person that provided the information about the taking of this medication")]
        public PractitionerGeneralInfo Prescriber { get; set; }

        [Description("The value and unit of the dosage")]
        [Required]
        public Dosage[] Dosages { get; set; }
    }
}

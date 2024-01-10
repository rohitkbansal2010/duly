// <copyright file="Dosage.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("How the medication is/was taken or should be taken")]
    public class Dosage
    {
        [Description("True if taken as needed (PRN). False otherwise")]
        public bool AsNeeded { get; set; }

        [Description("Amount of medication per dose. The amount of therapeutic or other substance given at one administration event")]
        public Quantity DoseQuantity { get; set; }

        [Description("The patient instructions for the prescription")]
        public string PatientInstruction { get; set; }

        [Description("The patient instructions for the prescription")]
        public string Text { get; set; }

        [Description("The frequency")]
        public Timing Timing { get; set; }
    }
}
// <copyright file="Medication.Reason.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("Condition or observation that supports why the medication is being/was taken")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    public class MedicationReason
    {
        [Description("Readable text with explanation")]
        [Required]
        public string[] ReasonText { get; set; }
    }
}
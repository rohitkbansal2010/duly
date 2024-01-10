// <copyright file="Medication.Reason.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Condition or observation that supports why the medication is being/was taken.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Models")]
    internal class MedicationReason
    {
        /// <summary>
        /// Readable text with explanation.
        /// </summary>
        public string[] ReasonText { get; set; }
    }
}
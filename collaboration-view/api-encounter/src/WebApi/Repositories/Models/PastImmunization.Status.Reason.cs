// <copyright file="PastImmunization.Status.Reason.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Explains why immunization did not occur.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    internal class PastImmunizationStatusReason
    {
        /// <summary>
        /// Readable text with explanation.
        /// </summary>
        public string Reason { get; set; }
    }
}

// <copyright file="PastImmunization.Vaccine.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Information about the vaccine for past immunization.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Contract")]
    internal class PastImmunizationVaccine
    {
        /// <summary>
        /// CVX codes of the vaccine substance administered.
        /// </summary>
        public string[] CvxCodes { get; set; }

        /// <summary>
        /// Name of vaccine product administered.
        /// </summary>
        public string Text { get; set; }
    }
}

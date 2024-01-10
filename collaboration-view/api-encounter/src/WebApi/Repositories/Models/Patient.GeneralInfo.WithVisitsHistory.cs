// -----------------------------------------------------------------------
// <copyright file="Patient.GeneralInfo.WithVisitsHistory.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Patient general information with visits history.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    internal class PatientGeneralInfoWithVisitsHistory
    {
        /// <summary>
        /// Patient general information.
        /// </summary>
        public PatientGeneralInfo Patient { get; set; }

        /// <summary>
        /// Patient information about past visits.
        /// </summary>
        public bool HasPastVisits { get; set; }
    }
}

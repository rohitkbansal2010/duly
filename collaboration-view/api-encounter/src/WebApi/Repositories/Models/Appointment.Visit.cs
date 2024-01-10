// -----------------------------------------------------------------------
// <copyright file="Appointment.Visit.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// A visit which specifies the appointment.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    internal class AppointmentVisit
    {
        /// <summary>
        /// Visit type display name such as "8014".
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// Visit type display name such as "NEW PT -SPECIALTY CONSULT".
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Visit type display name such as "New Patient Office Visit".
        /// </summary>
        public string TypeDisplayName { get; set; }
    }
}
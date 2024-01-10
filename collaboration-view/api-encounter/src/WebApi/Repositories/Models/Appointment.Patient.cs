// <copyright file="Appointment.Patient.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Patient details retrieved from NGDP.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    internal class AppointmentPatient
    {
        /// <summary>
        /// Identifier by which this patient is known.
        /// </summary>
        public string Id { get; set; }
    }
}

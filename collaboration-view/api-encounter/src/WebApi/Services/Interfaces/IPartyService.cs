// <copyright file="IPartyService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// Gets and orders all parties related to patient and appointment.
    /// </summary>
    public interface IPartyService
    {
        /// <summary>
        /// Gets and orders all parties related to patient and appointment.
        /// </summary>
        /// <param name="patientId"> Id of patient in question.</param>
        /// <param name="appointmentId"> Id of appointment in question.</param>
        /// <returns>Collection of Parties.</returns>
        public Task<IEnumerable<Contracts.Party>> GetPartiesByPatientAndAppointmentIdAsync(string patientId, string appointmentId);
    }
}
// <copyright file="IVitalService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="VitalsCard"/> entity and <see cref="VitalHistory"/>.
    /// </summary>
    public interface IVitalService
    {
        /// <summary>
        /// Retrieve the latest available <see cref="VitalsCard"/> items that match with the filtering parameter.
        /// </summary>
        /// <param name="patientId">Identifier of patient.</param>
        /// <returns>Filtered items of <see cref="VitalsCard"/>.</returns>
        Task<IEnumerable<VitalsCard>> GetLatestVitalsForPatientAsync(string patientId);

        /// <summary>
        /// Retrieve an available <see cref="VitalHistory"/> that matches with the filtering parameter.
        /// </summary>
        /// <param name="patientId">Identifier of patient.</param>
        /// <param name="startDate">The start date. Upper bound of required Vitals history.</param>
        /// <param name="vitalsCardType">Vital types filter.</param>
        /// <returns>Filtered <see cref="VitalHistory"/>.</returns>
        Task<VitalHistory> GetVitalHistoryForPatientByVitalsCardType(string patientId, DateTime startDate, VitalsCardType vitalsCardType);
    }
}
// <copyright file="IAllergyService.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Services.Interfaces
{
    /// <summary>
    /// A service that implements business logic according to
    /// the corresponding contract for the <see cref="Allergy"/> entity.
    /// </summary>
    public interface IAllergyService
    {
        /// <summary>
        /// Retrieve <see cref="Allergy"/> items that match with the filtering parameter.
        /// </summary>
        /// <param name="patientId">Identifier of patient.</param>
        /// <returns>Filtered items of <see cref="Allergy"/>.</returns>
        Task<IEnumerable<Allergy>> GetAllergiesForPatientAsync(string patientId);
    }
}

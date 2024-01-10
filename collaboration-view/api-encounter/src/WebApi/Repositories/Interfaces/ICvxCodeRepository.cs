// <copyright file="ICvxCodeRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working with CVX codes.
    /// </summary>
    internal interface ICvxCodeRepository
    {
        /// <summary>
        /// Retrieves a dictionary that contains all recognized vaccine group names by CVX codes (one group name per CVX code).
        /// </summary>
        /// <param name="codes">Array of CVX codes.</param>
        /// <returns>The dictionary, where each KeyValuePair.Key is a CVX code> and
        /// KeyValuePair.Value is a vaccine group name.</returns>
        Task<IReadOnlyDictionary<string, string>> FindVaccineGroupNamesByCodesAsync(string[] codes);
    }
}

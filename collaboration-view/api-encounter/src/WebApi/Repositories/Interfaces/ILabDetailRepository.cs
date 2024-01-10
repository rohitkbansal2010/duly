// <copyright file="ILabDetailRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.CheckOut.LabDetails"/> model.
    /// </summary>
    internal interface ILabDetailRepository
    {
        Task<int> PostLabDetailAsync(Models.CheckOut.LabDetails request);

        public Task<IEnumerable<Models.LabLocation>> GetLabLocationByLatLngAsync(string lat, string lng);
    }
}

// <copyright file="IImagingDetailRepository.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for working on <see cref="Models.CheckOut.ImagingDetails"/> model.
    /// </summary>
    internal interface IImagingDetailRepository
    {
        Task<int> PostImagingDetailAsync(Models.CheckOut.ImagingDetails request);
    }
}
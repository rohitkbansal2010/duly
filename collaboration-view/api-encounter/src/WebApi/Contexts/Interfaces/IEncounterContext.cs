// <copyright file="IEncounterContext.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Contexts.Interfaces
{
    /// <summary>
    /// Context for Encounter API.
    /// </summary>
    public interface IEncounterContext
    {
        /// <summary>
        /// Get the unique identifier of the request from the <see cref="Common.Infrastructure.Constants.ParameterNames.XCorrelationIdHeader" /> header.
        /// </summary>
        Guid GetXCorrelationId();
    }
}

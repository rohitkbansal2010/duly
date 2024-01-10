// <copyright file="IResourceContext.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Resource.Api.Contexts.Interfaces
{
    /// <summary>
    /// Context for Resource API.
    /// </summary>
    public interface IResourceContext
    {
        /// <summary>
        /// Get the unique identifier of the request from the <see cref="Common.Infrastructure.Constants.ParameterNames.XCorrelationIdHeader" /> header.
        /// </summary>
        Guid GetXCorrelationId();
    }
}

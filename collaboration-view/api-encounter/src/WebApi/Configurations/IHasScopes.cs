// -----------------------------------------------------------------------
// <copyright file="IHasScopes.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Configurations
{
    /// <summary>
    /// Configuration options for the API clients.
    /// </summary>
    public interface IHasScopes
    {
        /// <summary>
        /// Scopes.
        /// </summary>
        public string[] Scopes { get; }
    }
}
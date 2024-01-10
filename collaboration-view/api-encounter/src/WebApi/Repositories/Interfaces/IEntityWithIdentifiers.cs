// <copyright file="IEntityWithIdentifiers.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Interfaces
{
    /// <summary>
    /// Entity with identifiers collection.
    /// </summary>
    internal interface IEntityWithIdentifiers
    {
        /// <summary>
        /// Identifiers of the entity. Format: (Text|VALUE).
        /// </summary>
        public string[] Identifiers { get; }
    }
}

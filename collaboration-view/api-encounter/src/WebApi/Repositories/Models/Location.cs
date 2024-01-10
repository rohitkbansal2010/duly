// -----------------------------------------------------------------------
// <copyright file="Location.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Represents an information about a location.
    /// </summary>
    internal class Location
    {
        /// <summary>
        /// Title of the location. It could be a number, or another human readable name of the location.
        /// </summary>
        public string Title { get; set; }
    }
}

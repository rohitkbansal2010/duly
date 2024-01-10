// <copyright file="Observation.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Information about the observation.
    /// </summary>
    internal class Observation
    {
        /// <summary>
        /// Type of the observation. <see cref="ObservationType"/>.
        /// </summary>
        public ObservationType Type { get; set; }

        /// <summary>
        /// Arrays of <see cref="ObservationComponent"/>.
        /// Must exist at least one component.
        /// </summary>
        public ObservationComponent[] Components { get; set; }

        /// <summary>
        /// Date of the observation.
        /// </summary>
        public DateTimeOffset Date { get; set; }
    }
}

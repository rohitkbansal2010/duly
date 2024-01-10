// <copyright file="Observation.LabResult.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Information about the Lab result observation.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    internal class ObservationLabResult
    {
        /// <summary>
        /// Observation Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Component Name.
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// Observation components.
        /// </summary>
        public ObservationLabResultComponent[] Components { get; set; }

        /// <summary>
        /// Date of the observation.
        /// </summary>
        public DateTimeOffset Date { get; set; }
    }
}

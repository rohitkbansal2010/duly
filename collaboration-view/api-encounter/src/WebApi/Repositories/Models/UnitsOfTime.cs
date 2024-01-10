// <copyright file="UnitsOfTime.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// A unit of time (units from UCUM).
    /// (url: http://hl7.org/fhir/ValueSet/units-of-time)
    /// (systems: 0).
    /// </summary>
    public enum UnitsOfTime
    {
        /// <summary>
        /// Second.
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        S,

        /// <summary>
        /// Minute.
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        Min,

        /// <summary>
        /// Hour.
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        H,

        /// <summary>
        /// Day.
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        D,

        /// <summary>
        /// Week.
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        Wk,

        /// <summary>
        /// Month.
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        Mo,

        /// <summary>
        /// Year.
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        A,
    }
}
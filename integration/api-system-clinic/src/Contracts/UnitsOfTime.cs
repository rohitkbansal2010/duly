// <copyright file="UnitsOfTime.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    /// <summary>
    /// A unit of time (units from UCUM).
    /// (url: http://hl7.org/fhir/ValueSet/units-of-time)
    /// (systems: 0).
    /// </summary>
    [Description("A unit of time (units from UCUM).")]
    public enum UnitsOfTime
    {
        /// <summary>
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        [Description("second")]
        S,

        /// <summary>
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        [Description("minute")]
        Min,

        /// <summary>
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        [Description("hour")]
        H,

        /// <summary>
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        [Description("day")]
        D,

        /// <summary>
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        [Description("week")]
        Wk,

        /// <summary>
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        [Description("month")]
        Mo,

        /// <summary>
        /// (system: http://unitsofmeasure.org)
        /// </summary>
        [Description("year")]
        A,
    }
}
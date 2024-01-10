// <copyright file="Repeat.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// A set of rules that describe when the event is scheduled.
    /// </summary>
    internal class Repeat
    {
        /// <summary>
        /// The start and end dates for the medication.
        /// </summary>
        public Period BoundsPeriod { get; set; }

        /// <summary>
        /// If the medication is not PRN, and CountType is Doses, this element is populated with the count value.
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// If the medication is PRN, this element is set to Count.
        /// </summary>
        public int? CountMax { get; set; }

        /// <summary>
        /// The days of the week.
        /// </summary>
        public DaysOfWeek[] DaysOfWeek { get; set; }

        /// <summary>
        /// The medication duration.
        /// </summary>
        public decimal? Duration { get; set; }

        /// <summary>
        /// The frequency.
        /// </summary>
        public int? Frequency { get; set; }

        /// <summary>
        /// The period for taking the medication.
        /// </summary>
        public decimal? Period { get; set; }

        /// <summary>
        /// The period unit.
        /// </summary>
        public UnitsOfTime? PeriodUnit { get; set; }

        /// <summary>
        /// The time of day.
        /// </summary>
        public string[] TimesOfDay { get; set; }

        /// <summary>
        /// The administration timing.
        /// </summary>
        public EventTiming[] When { get; set; }
    }
}
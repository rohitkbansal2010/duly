// <copyright file="Repeat.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [Description("A set of rules that describe when the event is scheduled")]
    public class Repeat
    {
        [Description("The start and end dates for the medication")]
        public Period BoundsPeriod { get; set; }

        [Description("If the medication is not PRN, and CountType is Doses, this element is populated with the count value")]
        public int? Count { get; set; }

        [Description("If the medication is PRN, this element is set to Count")]
        public int? CountMax { get; set; }

        [Description("The days of the week")]
        [Required]
        public DaysOfWeek[] DaysOfWeek { get; set; }

        [Description("The medication duration")]
        public decimal? Duration { get; set; }

        [Description("The frequency")]
        public int? Frequency { get; set; }

        [Description("The period for taking the medication")]
        public decimal? Period { get; set; }

        [Description("The period unit")]
        public UnitsOfTime? PeriodUnit { get; set; }

        [Description("The times of day")]
        [Required]
        public string[] TimesOfDay { get; set; }

        [Description("The administration timing")]
        [Required]
        public EventTiming[] When { get; set; }
    }
}
// <copyright file="OfficialTimeZone.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Geographic region with same standard time")]
    public class OfficialTimeZone
    {
        [Description("Number")]
        public string Number { get; set; }

        [Description("Title")]
        public string Title { get; set; }

        [Description("Abbreviation")]
        public string Abbreviation { get; set; }
    }
}

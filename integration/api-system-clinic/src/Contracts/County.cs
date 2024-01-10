// <copyright file="County.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Part of state")]
    public class County
    {
        [Description("Number")]
        public string Number { get; set; }

        [Description("Title")]
        public string Title { get; set; }

        [Description("Abbreviation")]
        public string Abbreviation { get; set; }
    }
}
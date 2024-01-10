// <copyright file="Phone.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Contact phone")]
    public class Phone
    {
        [Description("Type")]
        public string Type { get; set; }

        [Description("Number")]
        public string Number { get; set; }
    }
}

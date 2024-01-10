// <copyright file="Quantity.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("A measured amount (or an amount that can potentially be measured)")]
    public class Quantity
    {
        [Description("Numerical value (with implicit precision)")]
        public decimal? Value { get; set; }

        [Description("Unit representation")]
        public string Unit { get; set; }
    }
}
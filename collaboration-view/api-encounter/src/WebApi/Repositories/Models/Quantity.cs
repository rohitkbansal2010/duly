// <copyright file="Quantity.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// A measured amount (or an amount that can potentially be measured).
    /// </summary>
    internal class Quantity
    {
        /// <summary>
        /// Numerical value (with implicit precision).
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Unit representation.
        /// </summary>
        public string Unit { get; set; }
    }
}
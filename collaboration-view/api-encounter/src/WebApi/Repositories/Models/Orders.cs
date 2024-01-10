// <copyright file="Orders.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Data of tests.
    /// </summary>
    public class Orders
    {
        /// <summary>
        /// Name of the Orders.
        /// </summary>
        public string OrderName { get; set; }

        /// <summary>
        /// Number of Orders.
        /// </summary>
        public string AuthoredOn { get; set; }
    }
}
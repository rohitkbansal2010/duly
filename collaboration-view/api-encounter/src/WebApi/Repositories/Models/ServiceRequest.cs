// <copyright file="ServiceRequest.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Data of tests.
    /// </summary>
    public class ServiceRequest
    {
        /// <summary>
        /// Name of the Orders.
        /// </summary>
        public List<Orders> TestOrder { get; set; }

        /// <summary>
        /// Number of Orders.
        /// </summary>
        public int? OrderCount { get; set; }
    }
}
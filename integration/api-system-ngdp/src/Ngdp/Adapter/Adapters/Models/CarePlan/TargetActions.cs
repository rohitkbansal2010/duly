// <copyright file="TargetActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Target actions for specific condition target.
    /// </summary>
    public class TargetActions
    {
        /// <summary>
        /// Action Id of target action.
        /// </summary>
        public long ActionId { get; set; }

        /// <summary>
        /// Target Id of target action.
        /// </summary>
        public long TargetId { get; set; }

        /// <summary>
        /// Action Name of target action.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Action Description of target action.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Notes of target action.
        /// </summary>
        public string Notes { get; set; }
    }
}
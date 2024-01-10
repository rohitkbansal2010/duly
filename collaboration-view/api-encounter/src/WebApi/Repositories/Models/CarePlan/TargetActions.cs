// <copyright file="TargetActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
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
        /// Action Name of target action.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Action Description of target action.
        /// </summary>
        public string Description { get; set; }
    }
}
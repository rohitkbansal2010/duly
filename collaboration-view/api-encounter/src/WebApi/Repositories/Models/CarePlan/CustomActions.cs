// <copyright file="CustomActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Custom Actions.
    /// </summary>
    public class CustomActions
    {
        /// <summary>
        /// Patient Target Identifier.
        /// </summary>
        public long PatientTargetId { get; set; }

        /// <summary>
        /// Action Name of condition targets.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Description of action.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Whether the custom action is selected or not.
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
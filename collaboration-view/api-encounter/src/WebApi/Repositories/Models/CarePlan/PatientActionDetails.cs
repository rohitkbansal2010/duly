// <copyright file="PatientActionDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Text.Json.Serialization;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Patient Action Details.
    /// </summary>
    public class PatientActionDetails
    {
        /// <summary>
        /// Patient Action Identifier.
        /// </summary>
        public long PatientActionId { get; set; }

        /// <summary>
        /// Patient Action Name.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Custom Action.
        /// </summary>
        public string CustomAction { get; set; }

        /// <summary>
        /// Custom Action Id.
        /// </summary>
        public long CustomActionId { get; set; }
    }
}
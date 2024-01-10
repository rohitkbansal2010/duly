// <copyright file="GetPatientActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Actions of Targets.
    /// </summary>
    public class GetPatientActions
    {
        /// <summary>
        /// Patient Action Id.
        /// </summary>
        public long PatientActionId { get; set; }

        /// <summary>
        /// Action Id.
        /// </summary>
        public long ActionId { get; set; }

        /// <summary>
        /// Custom Action Id    .
        /// </summary>
        public long CustomActionId { get; set; }

        /// <summary>
        /// Action Name.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Action Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Is Selected    .
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
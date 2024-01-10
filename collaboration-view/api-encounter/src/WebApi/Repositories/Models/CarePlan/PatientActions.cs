// <copyright file="PatientActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Actions of Targets.
    /// </summary>
    public class PatientActions
        {
        /// <summary>
        /// PatiendActionId of patient.
        /// </summary>
        public long PatientActionId { get; set; }

        /// <summary>
        /// PatiendTargetId of patient.
        /// </summary>
        public long PatientTargetId { get; set; }

        /// <summary>
        /// ActionId of Actions.
        /// </summary>
        public long ActionId { get; set; }

        /// <summary>
        /// Custom Id of the Actions.
        /// </summary>
        public long CustomActionId { get; set; }

        /// <summary>
        /// Deleted.
        /// </summary>
        public bool Deleted { get; set; }
    }
}
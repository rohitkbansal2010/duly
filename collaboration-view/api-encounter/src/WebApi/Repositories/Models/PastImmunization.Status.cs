// <copyright file="PastImmunization.Status.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Past immunization status codes.
    /// </summary>
    internal enum PastImmunizationStatus
    {
        /// <summary>
        /// The immunization was completed.
        /// </summary>
        Completed,

        /// <summary>
        /// The immunization was not done.
        /// </summary>
        NotDone,

        /// <summary>
        /// The immunization was entered in error.
        /// </summary>
        EnteredInError
    }
}
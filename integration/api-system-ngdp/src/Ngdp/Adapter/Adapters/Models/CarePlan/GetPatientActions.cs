// <copyright file="GetPatientActions.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Patient Action Details.
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
        /// Action Name.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Action Description.
        /// </summary>
        public string ActionDescription { get; set; }
    }
}
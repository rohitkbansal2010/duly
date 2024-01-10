// <copyright file="PatientActionDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Patient Action Details.
    /// </summary>
    public class PatientActionDetails
    {
        /// <summary>
        /// Patient Action Id.
        /// </summary>
        public long PatientActionId { get; set; }
        /// <summary>
        /// Action Name.
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// Custom Action Name.
        /// </summary>
        public string CustomActionName { get; set; }
    }
}
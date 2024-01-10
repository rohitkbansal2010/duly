// <copyright file="PatientTargetDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;

namespace Duly.Ngdp.Adapter.Adapters.Models.CarePlan
{
    /// <summary>
    /// Patient Target Details.
    /// </summary>
    public class PatientTargetDetails
    {
        /// <summary>
        /// Patient Target Id.
        /// </summary>
        public long PatientTargetId { get; set; }
        /// <summary>
        /// Target Namw.
        /// </summary>
        public string TargetName { get; set; }
        /// <summary>
        /// Custom Target Name.
        /// </summary>
        public string CustomTargetName { get; set; }
        /// <summary>
        /// Patient Action Details.
        /// </summary>
        public PatientActionDetails PatientActionDetails { get; set; }
    }
}
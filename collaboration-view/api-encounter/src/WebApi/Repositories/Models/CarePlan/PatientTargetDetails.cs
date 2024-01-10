// <copyright file="PatientTargetDetails.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Patient Target Details.
    /// </summary>
    public class PatientTargetDetails
    {
        /// <summary>
        /// Patient Target Identifier.
        /// </summary>
        public long PatientTargetId { get; set; }

        /// <summary>
        /// Patient Target Name.
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// Custom Target Id.
        /// </summary>
        public long CustomTargetId { get; set; }

        /// <summary>
        /// Custom Target.
        /// </summary>
        public string CustomTarget { get; set; }

        /// <summary>
        /// Patient Action Details.
        /// </summary>
        public PatientActionDetails PatientActionDetails { get; set; }
    }
}
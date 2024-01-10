// <copyright file="DeletePatientLifeGoalResponse.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Text.Json.Serialization;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Model for Delete Patient Life Goal Response.
    /// </summary>
    public class DeletePatientLifeGoalResponse
    {
        /// <summary>
        /// Patient Life Goal Identifier.
        /// </summary>
        public long PatientLifeGoalId { get; set; }

        /// <summary>
        /// Status Code.
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// Message.
        /// </summary>
        public string Message { get; set; }
    }
}
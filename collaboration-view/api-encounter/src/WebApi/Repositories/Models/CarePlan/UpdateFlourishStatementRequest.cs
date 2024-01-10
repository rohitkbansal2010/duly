// <copyright file="UpdateFlourishStatementRequest.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    public class UpdateFlourishStatementRequest
    {
        /// <summary>
        /// Patient Plan Identifier.
        /// </summary>
        public long PatientPlanId { get; set; }

        /// <summary>
        /// Flourish Statement.
        /// </summary>
        public string FlourishStatement { get; set; }
    }
}
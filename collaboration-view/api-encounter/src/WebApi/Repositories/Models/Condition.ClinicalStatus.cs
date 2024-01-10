// <copyright file="Condition.ClinicalStatus.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Condition Status Codes.
    /// </summary>
    internal enum ConditionClinicalStatus
    {
        /// <summary>
        /// The subject is currently experiencing the condition or situation, there is evidence of the condition or situation, or considered to be a significant risk.
        /// </summary>
        Active,

        /// <summary>
        /// The subject is no longer experiencing the condition or situation and there is no longer evidence or appreciable risk of the condition or situation.
        /// </summary>
        Inactive,

        /// <summary>
        /// The subject is not presently experiencing the condition or situation and there is a negligible perceived risk of the condition or situation returning.
        /// </summary>
        Resolved
    }
}

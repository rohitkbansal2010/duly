// <copyright file="CareTeam.Category.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Indicates the type of care team.
    /// </summary>
    public enum CareTeamCategory
    {
        /// <summary>
        /// Longitudinal (Longitudinal care-coordination focused care team).
        /// </summary>
        Longitudinal,

        /// <summary>
        /// Episode (Episode of care-focused care team).
        /// </summary>
        Episode
    }
}
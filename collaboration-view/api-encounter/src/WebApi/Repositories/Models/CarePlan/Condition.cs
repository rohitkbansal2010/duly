// <copyright file="Condition.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System;
using System.Text.Json.Serialization;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.CarePlan
{
    /// <summary>
    /// Condition.
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// Condition Id of condition.
        /// </summary>
        public long ConditionId { get; set; }

        /// <summary>
        /// Condition short name.
        /// </summary>
        public string ConditionShortName { get; set; }

        /// <summary>
        /// Condition display name.
        /// </summary>
        public string ConditionDisplayName { get; set; }

        /// <summary>
        /// Active.
        /// </summary>
        public bool Active { get; set; }
    }
}
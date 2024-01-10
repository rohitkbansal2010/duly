// <copyright file="HumanGeneralInfo.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models.Abstractions
{
    internal abstract class HumanGeneralInfo
    {
        /// <summary>
        /// A human identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A name associated with the human.
        /// </summary>
        public HumanName[] Names { get; set; }
    }
}
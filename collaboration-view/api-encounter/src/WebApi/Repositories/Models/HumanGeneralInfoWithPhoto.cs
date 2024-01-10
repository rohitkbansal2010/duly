// <copyright file="HumanGeneralInfoWithPhoto.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Repositories.Models.Abstractions;

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Abstraction of a human name with photo.
    /// </summary>
    internal class HumanGeneralInfoWithPhoto : HumanGeneralInfo
    {
        /// <summary>
        /// Image of the human.
        /// </summary>
        public Attachment[] Photos { get; set; }
    }
}
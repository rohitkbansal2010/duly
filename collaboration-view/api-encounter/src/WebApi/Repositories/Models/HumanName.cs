// -----------------------------------------------------------------------
// <copyright file="HumanName.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// A name of a human with text, parts.
    /// </summary>
    internal class HumanName
    {
        /// <summary>
        /// Family name (often called 'Surname').
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Given names (not always 'first'). Includes middle names. This repeating element order: Given Names appear in the correct order for presenting the name.
        /// </summary>
        public string[] GivenNames { get; set; }

        /// <summary>
        /// Parts that come before the name. This repeating element order: Prefixes appear in the correct order for presenting the name.
        /// </summary>
        public string[] Prefixes { get; set; }

        /// <summary>
        /// Parts that come after the name. This repeating element order: Suffixes appear in the correct order for presenting the name.
        /// </summary>
        public string[] Suffixes { get; set; }

        /// <summary>
        /// The purpose of the name.
        /// </summary>
        public NameUse Use { get; set; }
    }
}
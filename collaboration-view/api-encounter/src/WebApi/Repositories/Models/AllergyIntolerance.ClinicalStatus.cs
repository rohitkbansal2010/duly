// <copyright file="AllergyIntolerance.ClinicalStatus.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    /// <summary>
    /// Allergy intolerance clinical status Codes.
    /// </summary>
    internal enum AllergyIntoleranceClinicalStatus
    {
        /// <summary>
        /// The subject is currently experiencing, or is at risk of, a reaction to the identified substance.
        /// </summary>
        Active = 1,

        /// <summary>
        /// A reaction to the identified substance has been clinically reassessed by testing or re-exposure and is considered no longer to be present. Re-exposure could be accidental, unplanned, or outside of any clinical setting.
        /// </summary>
        Resolved = 2,

        /// <summary>
        /// The subject is no longer at risk of a reaction to the identified substance.
        /// </summary>
        Inactive = 0
    }
}

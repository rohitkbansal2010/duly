// <copyright file="AllergyIntolerance.Category.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

namespace Duly.CollaborationView.Encounter.Api.Repositories.Models
{
    internal enum AllergyIntoleranceCategory
    {
        /// <summary>
        /// Any substance consumed to provide nutritional support for the body.
        /// </summary>
        Food,

        /// <summary>
        /// Substances administered to achieve a physiological effect.
        /// </summary>
        Medication,

        /// <summary>
        /// Any substances that are encountered in the environment.
        /// </summary>
        Environment,

        /// <summary>
        /// A preparation that is synthesized from living organisms or their products.
        /// </summary>
        Biologic,

        /// <summary>
        /// Uncategorized item.
        /// </summary>
        Other
    }
}

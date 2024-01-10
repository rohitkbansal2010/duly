// -----------------------------------------------------------------------
// <copyright file="AllergyIntolerance.Category.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [Description("Category of an identified substance associated with allergy intolerances.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1602:Enumeration items should be documented",
        Justification = "Contract")]
    public enum AllergyIntoleranceCategory
    {
        [Description("Any substance consumed to provide nutritional support for the body.")]
        Food,

        [Description("Substances administered to achieve a physiological effect.")]
        Medication,

        [Description("Any substances that are encountered in the environment.")]
        Environment,

        [Description("A preparation that is synthesized from living organisms or their products.")]
        Biologic,

        [Description("Uncategorized item")]
        Other
    }
}

// <copyright file="RelatedPerson.GeneralInfo.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    [Description("Information about a person that is involved in the care for a patient, but who is not the target of healthcare, nor has a formal responsibility in the care process.")]
    public class RelatedPersonGeneralInfo : HumanGeneralInfoWithPhoto
    {
    }
}
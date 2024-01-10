// <copyright file="ClinicalStatus.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Contract")]
    [Description("Clinical status of allergy")]

    //From https://www.hl7.org/fhir/valueset-allergyintolerance-clinical.html
    public enum ClinicalStatus
    {
        [Description("The subject is currently experiencing, or is at risk of, a reaction to the identified substance.")]
        Active
    }
}
// -----------------------------------------------------------------------
// <copyright file="Allergy.ClinicalStatus.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [Description("Clinical status of allergy")]

    //From https://www.hl7.org/fhir/valueset-allergyintolerance-clinical.html
    public enum ClinicalStatus
    {
        [Description("The subject is currently experiencing, or is at risk of, a reaction to the identified substance.")]
        Active = 1,

        [Description("A reaction to the identified substance has been clinically reassessed by testing or re-exposure and is considered no longer to be present. Re-exposure could be accidental, unplanned, or outside of any clinical setting.")]
        Resolved = 2,

        [Description("The subject is no longer at risk of a reaction to the identified substance.")]
        Inactive = 0
    }
}
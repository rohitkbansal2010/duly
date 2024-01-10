// -----------------------------------------------------------------------
// <copyright file="Patient.GeneralInfo.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    [Description("Short Information about an individual receiving health care services")]
    public class PatientGeneralInfo
    {
        [Description("An identifier for the patient")]
        [Required]
        public string Id { get; set; }

        [Description("A name associated with the patient")]
        [Required]
        public HumanName HumanName { get; set; }
    }
}
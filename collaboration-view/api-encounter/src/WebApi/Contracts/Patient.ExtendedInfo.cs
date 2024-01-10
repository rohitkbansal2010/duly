// -----------------------------------------------------------------------
// <copyright file="Patient.ExtendedInfo.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contract")]
    [Description("Patient extended information")]
    public class PatientExtendedInfo
    {
        [Description("Patient general information")]
        [Required]
        public PatientGeneralInfo PatientGeneralInfo { get; set; }

        [Description("Flag if patient is new")]
        public bool IsNewPatient { get; set; }
    }
}

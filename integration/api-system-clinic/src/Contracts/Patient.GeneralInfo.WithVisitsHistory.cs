// <copyright file="Patient.GeneralInfo.WithVisitsHistory.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Duly.Clinic.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    [Description("Patient general information with visits history")]
    public class PatientGeneralInfoWithVisitsHistory : IDulyResource
    {
        [Description("Patient general information")]
        [Required]
        public PatientGeneralInfo Patient { get; set; }

        [Description("Patient information about past visits")]
        public bool HasPastVisits { get; set; }
    }
}
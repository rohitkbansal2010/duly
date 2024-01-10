// <copyright file="Patient.GeneralInfo.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts.Abstractions;
using Duly.Clinic.Contracts.Interfaces;
using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    [Description("Information about an individual receiving health care services")]
    public class PatientGeneralInfo : HumanGeneralInfo, IDulyResource
    {
    }
}

// -----------------------------------------------------------------------
// <copyright file="Practitioner.GeneralInfo.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Duly.Clinic.Contracts.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Contracts")]
    [Description("A person who is directly or indirectly involved in the provisioning of healthcare")]
    public class PractitionerGeneralInfo : HumanGeneralInfoWithPhoto, IDulyResource
    {
        [Description("Roles which this practitioner may perform")]
        public Role[] Roles { get; set; }

        [Description("Identifiers of the practitioners. Format: (Text|VALUE)")]
        public string[] Identifiers { get; set; }

        [Description("Speciality")]
        public List<string> Speciality { get; set; }
    }
}